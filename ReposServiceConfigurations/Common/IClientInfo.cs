namespace ReposServiceConfigure.Common
{
    public interface IClientInfo
    {
        string AssmPrefix { get; }
        string DefaultPrefix { get; }
        dynamic GetEnum(string enumName);
    }

    public interface IClientInfo<T>
        : IClientInfo
    {


    }
}
