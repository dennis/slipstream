using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            var _ = serviceProvider.GetService<PopulateSink>(); // HACK: Will inject EventBus/EventFactory into sink

            string cwd = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Slipstream\";
            System.IO.Directory.CreateDirectory(cwd);
            System.IO.Directory.SetCurrentDirectory(cwd);

            Start(serviceProvider);
        }

        private static void Start(ServiceProvider serviceProvider)
        {
            var engine = serviceProvider.GetRequiredService<Backend.IEngine>();

            engine.Start();

            Application.Run(serviceProvider.GetRequiredService<Frontend.MainWindow>());

            engine.Dispose();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<Frontend.MainWindow>();
            services.AddScoped<Shared.IEventBus, Backend.EventBus>();
            services.AddScoped<Shared.IEventProducer>(x => x.GetService<Backend.EventBus>());
            services.AddScoped<IServiceLocator, ServiceLocator>();
            services.AddScoped<Backend.IEngine, Backend.Engine>();
            services.AddScoped<Backend.Services.IStateService>(x => new Backend.Services.StateService(x.GetService<ILogger>(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Slipstream\state.txt"));
            services.AddScoped<Backend.Services.IEventSerdeService, Backend.Services.EventSerdeService>();
            services.AddScoped<Backend.PluginManager>();
            services.AddScoped<Backend.IPluginManager>(x => x.GetService<Backend.PluginManager>());
            services.AddScoped<Backend.IPluginFactory>(x => x.GetService<Backend.PluginManager>());
            services.AddScoped<EventHandlerControllerBuilder>();
            services.AddScoped<IEventFactory, EventFactory>();
            services.AddScoped<Backend.Services.ILuaSevice, Backend.Services.LuaService>();
            services.AddSingleton<Shared.IApplicationVersionService, Shared.ApplicationVersionService>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.SlipstreamConsoleSink(out SlipstreamConsoleSink sink)
                .CreateLogger();

            services.AddScoped<ILogger>(_ => logger);
            services.AddScoped<SlipstreamConsoleSink>(_ => sink);
            services.AddScoped<PopulateSink>();
        }

        private class PopulateSink
        {
            public PopulateSink(SlipstreamConsoleSink sink, IEventBus eventBus, IEventFactory eventFactory)
            {
                sink.EventBus = eventBus;
                sink.EventFactory = eventFactory.Get<IUIEventFactory>();
            }
        }
    }
}