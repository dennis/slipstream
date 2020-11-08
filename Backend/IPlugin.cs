namespace Slipstream.Backend
{
    interface IPlugin
    {
        public string Id { get; }
        public string Name { get; }
        public string DisplayName { get; }
        public bool Enabled { get; }
        public string WorkerName { get; }
        void RegisterPlugin(IEngine engine);
        void UnregisterPlugin(IEngine engine);
        void Enable(IEngine engine);
        void Disable(IEngine engine);
        void Loop();
    }
}
