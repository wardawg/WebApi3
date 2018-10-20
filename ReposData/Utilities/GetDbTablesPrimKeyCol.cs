//using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Design.PluralizationServices;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReposData.Utilities
{



    /// <summary>
    /// 
    /// </summary>
    public class GetDbHelperTablesPrimKeyCol
    {

        public class DbMappingTable
        {
            public string MappingTypeName = string.Empty;
            public string EntityName = string.Empty;
            public string TableName = string.Empty;
            public Type EntityType;
            public DbPkMapping PkMapping = new DbPkMapping();
            public int MappingOrder => Sort(MappingTypeName);
                                     

            private int Sort(string name)
            {
                int order = 0; 
                switch(name)
                {
                    case "IReposEntityType":
                        order = 1;
                    break;
                    case "IDomainEntityHandler":
                        order = 2;
                    break;
                    default:
                        order = int.MaxValue;
                        break;
                }

                return order;

            }
           // public int MappingOrder get() => 2;

        }

        public class DbPkMapping
        {
            public string TableName;
            public string PkColumnName;
            public string IndexName;
                               
        }

        IList<DbMappingTable> tblEntity = new List<DbMappingTable>();
        PluralizationService ps;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="conn"></param>
       /// <param name="types"></param>
       public GetDbHelperTablesPrimKeyCol(DbConnection conn,string contextName, List<EntityMapType> types)
       {
            CultureInfo ci = new CultureInfo("en-us");
            ps = PluralizationService.CreateService(ci);

            
            SetKeysFromDb(conn, contextName,types);
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="types"></param>
        private void SetKeysFromDb(DbConnection conn,string contextName, List<EntityMapType> types)
        {

        try { 


            DataTable fk = default(DataTable);
            List<DbPkMapping> tblPrimKey = new List<DbPkMapping>();

            if (conn.State == ConnectionState.Closed)
                conn.Open();

               var sql = @"select 
                            b.TABLE_NAME,b.COLUMN_NAME,a.CONSTRAINT_NAME
                            from
                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS a
                               ,INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE b
                            where
                                CONSTRAINT_TYPE = 'PRIMARY KEY'
                                and a.CONSTRAINT_NAME = b.CONSTRAINT_NAME";


                ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;


                var providerName = settings[contextName].ProviderName;

                if (providerName != "System.Data.SqlClient")
                {
                    fk = conn.GetSchema("IndexColumns");
                    var fi = conn.GetSchema("Indexes");
                    var PK_Column = default(String);
                    var INX_Name = default(String);


                    if (providerName == "MySql.Data.MySqlClient")
                    {
                        PK_Column = "PRIMARY";
                        INX_Name = "INDEX_NAME";
                    }
                    else
                    {
                        PK_Column = "PRIMARY_KEY";
                        INX_Name = "CONSTRAINT_NAME";
                    }

                    
                    foreach (DataRow row in fi.Rows)
                        if (Convert.ToBoolean(row[PK_Column]))
                            tblPrimKey.Add(
                                 fk.AsEnumerable()
                                 .Where(w => w["TABLE_NAME"].ToString() == row["TABLE_NAME"].ToString())
                                 .Select(s => new DbPkMapping
                                 {
                                     TableName = s["TABLE_NAME"].ToString()
                                    ,PkColumnName = s["COLUMN_NAME"].ToString()
                                    ,IndexName = s[INX_Name].ToString()
                                 }).FirstOrDefault());
                    
                }
                else
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    var result = cmd.ExecuteReader();
                     tblPrimKey = result
                                     .Cast<DbDataRecord>()
                                     .ToList()
                                     .Select(s => new DbPkMapping
                                     {
                                           TableName    = s["Table_Name"].ToString()
                                          ,PkColumnName = s["COLUMN_NAME"].ToString()
                                          ,IndexName    = s["CONSTRAINT_NAME"].ToString()
                                     }).ToList();
                }
 

Func<Type, string> DeriveTableNameEntity = (Type entity) =>
{

var tblName = default(string);

try
{
    tblName = entity
                     .BaseType
                     .GenericTypeArguments
                     .First()
                     .Name;
}
catch {
    tblName = entity.Name;
}

return tblName;
};

 
tblEntity =
types
.Where(s => !s.EntityType.GetCustomAttributes(typeof(DomainNoBindAttribute), true).Any())
.ToList()
.Select(s => new DbMappingTable
{
    EntityName = s.EntityType.Name
                , TableName = ps.Pluralize(DeriveTableNameEntity(s.EntityType))
                , PkMapping = tblPrimKey.Where(w=> w.TableName.ToLower() == ps.Pluralize(DeriveTableNameEntity(s.EntityType)).ToLower()).FirstOrDefault()
                , EntityType = s.EntityType
                , MappingTypeName = s.EntityTypeName
}
       ).ToList();

}
finally
{
conn.Close();
}


}


/// <summary>
/// Get Table Mapping
/// </summary>
/// <returns></returns>
public IEnumerable<DbMappingTable> GetEntityMapping()
{
return tblEntity != null ? tblEntity : new List<DbMappingTable>();
}

/// <summary>
/// Get Table Primary Key Column
/// </summary>
/// <param name="sEntity"></param>
/// <returns>string</returns>
public string GetTablePKey(string sEntity)
{
var sTableName = ps.Pluralize(sEntity);

DbMappingTable table = tblEntity
                    .FirstOrDefault(w => w.TableName == sTableName);

DbPkMapping pkMapping = default(DbPkMapping);

if (table != null)
pkMapping = table.PkMapping;

return pkMapping != null ? pkMapping.PkColumnName : sEntity + "Id";
}
}
}
