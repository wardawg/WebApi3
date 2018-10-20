using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repos.DomainModel.Interface.Interfaces
{
    public abstract partial class BaseEntity { }
    //: IBaseEntity //BaseEntity<NullDomain> { }

    public abstract partial class BaseEntity<T>
        : IBaseEntity<T>
        , IDomainRepository
    {
        private Guid _ObjectId = Guid.NewGuid();

        protected BaseEntity() {
        }
        protected BaseEntity(string RuleName) {
        }

        [NotMapped]
        public bool GenerateIndentity { set; get; }

        [NotMapped]
        public bool RulesEnabled { set; get; } = true;
        [NotMapped]
        public bool ApplyDefaults { set; get; } = true;

        [Required]
        [Column]
        [Key]
        public virtual int Id { get; protected set; }

        [NotMapped]
        public virtual bool IsNew {get {return Id == 0;} }
                
        [NotMapped]
        public T OldPicture { get; set; } = default(T);

        [NotMapped]
        public string ObjName
        {
            get { return typeof(T).Name; }
        }
                
        protected string ObjectId()
        {
            return _ObjectId.ToString();
        }

       
    }

}
