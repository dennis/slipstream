namespace Slipstream.Components
{
    internal interface IComponent
    {
        public void Register(IComponentRegistrationContext reg);
    }
}