using Autofac;
using Serilog;
using Slipstream.Components;
using Slipstream.Components.Internal;
using Slipstream.Components.Internal.Services;
using Slipstream.Shared;
using System;
using System.Windows.Forms;

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

            var builder = new ContainerBuilder();

            ConfigureServices(builder);

            string cwd = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Slipstream\";
            System.IO.Directory.CreateDirectory(cwd);
            System.IO.Directory.SetCurrentDirectory(cwd);

            using var container = builder.Build();

            Start(container);
        }

        private static void Start(IContainer container)
        {
            using var scope = container.BeginLifetimeScope();

            var _ = scope.Resolve<PopulateSink>(); // HACK: Will inject EventBus/EventFactory into sink

            using var engine = scope.Resolve<Backend.IEngine>();
            engine.Start();
        }

        private static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<Backend.EventBus>().As<Shared.IEventBus>().As<Shared.IEventProducer>().SingleInstance();
            builder.RegisterType<Backend.Engine>().As<Backend.IEngine>().SingleInstance();
            builder.RegisterType<EventSerdeService>().As<IEventSerdeService>().SingleInstance();
            builder.RegisterType<Backend.PluginManager>().As<Backend.IPluginManager>().As<Backend.IPluginFactory>().SingleInstance();
            builder.RegisterType<LuaService>().As<ILuaSevice>().SingleInstance();
            builder.RegisterType<Shared.ApplicationVersionService>().As<Shared.IApplicationVersionService>().SingleInstance();
            builder.RegisterType<PopulateSink>().SingleInstance();
            builder.Register(c => new StateService(c.Resolve<ILogger>(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Slipstream\state.txt")).As<IStateService>().SingleInstance();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.SlipstreamConsoleSink(out SlipstreamConsoleSink sink)
                .CreateLogger();

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            builder.RegisterInstance(sink).As<SlipstreamConsoleSink>().SingleInstance();

            // EventHandlers
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.IsAssignableTo<IEventHandler>())
                .Where(t => !t.IsAbstract)
                .As<IEventHandler>()
                .AsSelf()
                .InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.Name.EndsWith("EventFactory"))
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<EventHandlerController>().As<IEventHandlerController>().InstancePerDependency();

            // LuaGlues
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.IsAssignableTo<ILuaGlue>())
                .Where(t => !t.IsAbstract)
                .As<ILuaGlue>()
                .AsSelf()
                .InstancePerDependency();
        }

        private class PopulateSink
        {
            public PopulateSink(SlipstreamConsoleSink sink, IEventBus eventBus, Components.UI.IUIEventFactory uiEventFactory)
            {
                sink.EventBus = eventBus;
                sink.EventFactory = uiEventFactory;
            }
        }
    }
}