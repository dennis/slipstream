namespace Slipstream.Components.Internal
{
    public interface IServiceLocator
    {
        T Get<T>();

        void Add<T>(T service);
    }
}