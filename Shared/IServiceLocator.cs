namespace Slipstream.Shared
{
    public interface IServiceLocator
    {
        T Get<T>();

        void Add<T>(T service);
    }
}