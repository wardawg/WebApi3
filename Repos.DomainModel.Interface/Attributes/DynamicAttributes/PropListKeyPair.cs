namespace Repos.DomainModel.Interface.Atrributes.DynamicAttributes
{
    public class PropListKeyPair
        : AttributeList
    {
        public PropListKeyPair(){
        }

        public string Key   { set; get; }
        public object Value { set; get; }


        public PropListKeyPair(string key , object value)
        {
            Key = key;
            Value = value;
        }
                
    }
}