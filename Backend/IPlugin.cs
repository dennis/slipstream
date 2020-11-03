namespace Slipstream.Backend
{
    interface IPlugin
    {
        public string Name { get; }
        public bool Enabled { get; }
        void RegisterPlugin(IEngine engine);
        void UnregisterPlugin(IEngine engine);
        void Enable(IEngine engine);
        void Disable(IEngine engine);
    }
}
