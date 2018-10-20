using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Attributes;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;



namespace ReposDomain.Handlers.Handlers
{

    public interface IWebApiControllerHandler : IGenericHandler
    {

        IQueryable<WebApiControllerProxy> GetAll();
        IQueryable<WebApiControllerProxy> GetByClientId(int Id);
        //IEnumerable<WebApiControllerProxy> GetApiVersion(Expression<Func<WebApiControllerProxy, bool>> predicate = null);
        IEnumerable<WebApiControllerProxy> GetApiVersion(string ClientExtId);
        IEnumerable<WebApiController> GetControllers();
    }

    //[DomainNoBindAttribute]
   // [NoServiceResolveAtrribute]
    public class WebApiControllerHandler
       :  ServiceGenericHandler<WebApiController> 
        , IWebApiControllerHandler
    {
            
        private IRepository<ClientController> repos_client;
        private IRepository<WebApiController> repos_webapi;
        private IRepository<ClientRefInfo> repos_clientRef;
        private IRepository<WebApiCntlVersion> repos_version;


        public IEnumerable<WebApiController> GetControllers()
        {
            return repos_webapi.TableNoTracking;
        }

        public WebApiControllerHandler(IRepository<WebApiController> webApi
                                     , IRepository<ClientController> client
                                     , IRepository<ClientRefInfo> clientRef
                                     , IRepository<WebApiCntlVersion> webVersion
                                   //  , ICacheService cache
            )
            : base(webApi,null)
        {
            repos_client = client;
            repos_webapi = webApi;
            repos_clientRef = clientRef;
            repos_version = webVersion;
        }


        public IQueryable<WebApiControllerProxy> GetByClientId(int Id)
        {
            var qry1 = (from cc in repos_webapi.TableNoTracking
                        from rc in repos_client.TableNoTracking
                        from crf in repos_clientRef.TableNoTracking
                        where  rc.ControllerId == cc.Id
                               && rc.Id == crf.Id
                        join sub in
                                       (from wc in repos_webapi
                                                       .TableNoTracking
                                        from wv in repos_version
                                                        .TableNoTracking
                                        where wc.Id == wv.Id
                                        select new { wc.Id , wv.version}
                                       )
                           on cc.Id equals sub.Id into t
                           from rt in t.DefaultIfEmpty()
                           select new { crf.ExtClientId
                                        ,cc.Id
                                        , cc.controllername
                                        , version = rt.version ?? 1 });

            var qry2 = (from wv in repos_version.TableNoTracking
                        from rc in repos_client.TableNoTracking
                        from crf in repos_clientRef.TableNoTracking
                        where rc.ControllerId == wv.Id
                               && rc.Id == crf.Id

                        join sub in
                                    (from wc in repos_webapi
                                                    .TableNoTracking
                                     from wvx in repos_version
                                                     .TableNoTracking
                                     where wc.Id == wvx.Id
                                        select new { wc.Id, wc.controllername,wvx.version}
                                    )
                         on wv.Id equals sub.Id into t
                        from rt in t.DefaultIfEmpty()
                        select new { crf.ExtClientId
                                    , wv.Id
                                    , rt.controllername
                                    , version = rt.version ?? 1 });

            var controls = qry1
                            .Union(qry2)
                            .Select(s => new WebApiControllerProxy
                            {
                                Id = s.Id
                                ,clientId = Id
                                , controllername = s.controllername
                                ,ExtClientId = s.ExtClientId
                                ,version= s.version
                            });

            var qr3 = (from t in controls
                        join sub in repos_client.TableNoTracking
                               on t.Id equals sub.Id
                      select new WebApiControllerProxy
                      {
                         Id = t.Id
                        ,clientId = sub.Id
                        ,controllername = t.controllername
                        ,ExtClientId = t.ExtClientId
                        ,version = t.version
                      }
                      
                      );

            if (qr3.Any())
                return qr3.AsQueryable();


            //var c = qry1.Union(qry2).ToList();

            return controls.AsQueryable();

        }


        public IEnumerable<WebApiControllerProxy> GetApiVersion(string ClientExtId)
        //IEnumerable<WebApiControllerProxy> GetApiVersion(Expression<Func<WebApiControllerProxy, bool>> predicate = null)

        {

            Func<WebApiControllerProxy, bool> deg = i => true;
            Expression<Func<WebApiControllerProxy, bool>> expr = i => true;
            //var p = predicate == null ? expr : predicate;


            return GetAll(ClientExtId);
                //   .Where(p)
                //   .ToList();

        }


        public IQueryable<WebApiControllerProxy> GetAll()
        {
            return GetAll(null);
        }

        private IQueryable<WebApiControllerProxy> GetAll(string ExtClientId)
        {

            var qry1 = (from cc in repos_webapi.TableNoTracking
                        from rc in repos_client.TableNoTracking
                        from crf in repos_clientRef.TableNoTracking
                        where rc.ControllerId == cc.Id
                               && rc.Id == crf.Id
                               && crf.ExtClientId == (!string.IsNullOrEmpty(ExtClientId) ? ExtClientId : crf.ExtClientId)
                        join sub in
                                       (from wc in repos_webapi
                                                       .TableNoTracking
                                        from wv in repos_version
                                                        .TableNoTracking
                                        where wc.Id == wv.Id
                                        select new { wc.Id, wv.version}
                                       )
                           on cc.Id equals sub.Id into t
                        from rt in t.DefaultIfEmpty()
                        select new
                        {
                            ExtClientId = ExtClientId 
                            ,cc.Id
                            ,cc.controllername
                            ,version = rt.version ?? 1
                        });

            var qry2 = (from wv in repos_version.TableNoTracking
                        from rc in repos_client.TableNoTracking
                        from crf in repos_clientRef.TableNoTracking
                        where rc.ControllerId == wv.Id
                               && rc.Id == crf.Id

                        join sub in
                                    (from wc in repos_webapi
                                                    .TableNoTracking
                                     from wvx in repos_version
                                                     .TableNoTracking
                                     where wc.Id == wvx.Id
                                     select new { wc.Id, wc.controllername, wvx.version }
                                    )
                         on wv.Id equals sub.Id into t
                        from rt in t.DefaultIfEmpty()
                        select new
                        {
                            
                            crf.ExtClientId
                                    ,
                            wv.Id
                                    ,
                            rt.controllername
                                    ,
                            version = rt.version ?? 1
                        });

            var controls = qry1
                            .Union(qry2)
                            .Select(s => new WebApiControllerProxy
                            {
                                Id = s.Id
                                ,
                                clientId = Id
                                ,
                                controllername = s.controllername
                                ,
                                ExtClientId = s.ExtClientId
                                ,
                                version = s.version
                            });

            var qr3 = (from t in controls
                       join sub in repos_client.TableNoTracking
                              on t.Id equals sub.Id
                       select new WebApiControllerProxy
                       {
                           Id = t.Id
                         ,
                           clientId = sub.Id
                         ,
                           controllername = t.controllername
                         ,
                           ExtClientId = t.ExtClientId
                         ,
                           version = t.version
                       }

                      );

            if (qr3.Any())
                return qr3.AsQueryable();


            //var c = qry1.Union(qry2).ToList();

            return controls.AsQueryable();
        }

              
    }

    [NotMapped]
    public class WebApiControllerProxy
     
    {
       
        public int Id { set; get; }
        public int clientId { set; get; }
        public string controllername { get; set; }
        public int? version { get; set; }
        public string ExtClientId { set; get; }

        
    }
}


