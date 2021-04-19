using Autofac;
using Serilog;
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
            builder.Register(scope => scope.Resolve<IEventBus>().RegisterListener()).As<IEventBusSubscription>().InstancePerDependency();
            builder.RegisterType<Backend.Engine>().As<Backend.IEngine>().SingleInstance();
            builder.RegisterType<EventSerdeService>().As<IEventSerdeService>().SingleInstance();
            builder.RegisterType<Backend.PluginManager>().As<Backend.IPluginManager>().As<Backend.IPluginFactory>().SingleInstance();
            builder.RegisterType<LuaService>().As<ILuaService>().InstancePerDependency();
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

            // Plugins
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.IsAssignableTo<Components.IPlugin>())
                .Where(t => !t.IsAbstract)
                .As<Components.IPlugin>()
                .AsSelf()
                .InstancePerDependency();

            // Lua related classes
            builder.RegisterType<LuaLibraryRepository>().As<ILuaLibraryRepository>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.IsAssignableTo<ILuaLibrary>())
                .Where(t => !t.IsAbstract)
                .As<ILuaLibrary>()
                .AsSelf()
                .InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.IsAssignableTo<ILuaReference>())
                .Where(t => !t.IsAbstract)
                .As<ILuaReference>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<Components.Audio.Lua.AudioInstanceThread>().As<Components.Audio.Lua.IAudioInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.Discord.Lua.DiscordServiceThread>().As<Components.Discord.Lua.IDiscordInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.Discord.Lua.DiscordLuaChannelReference>().As<Components.Discord.Lua.IDiscordLuaChannelReference>().InstancePerDependency();
            builder.RegisterType<Components.FileMonitor.Lua.FileMonitorInstanceThread>().As<Components.FileMonitor.Lua.IFileMonitorInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.FileMonitor.Lua.FileMonitorLuaReference>().As<Components.FileMonitor.Lua.IFileMonitorLuaReference>().InstancePerDependency();
            builder.RegisterType<Components.Lua.Lua.LuaInstanceThread>().As<Components.Lua.Lua.ILuaInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.Lua.Lua.LuaLuaReference>().As<Components.Lua.Lua.ILuaLuaReference>().InstancePerDependency();
            builder.Register(scope => scope.Resolve<Components.Lua.Lua.LuaLuaLibrary>()).As<Components.Lua.Lua.ILuaLuaLibrary>();
            builder.RegisterType<Components.IRacing.Lua.IRacingInstanceThread>().As<Components.IRacing.Lua.IIRacingInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.IRacing.Lua.IRacingReference>().As<Components.IRacing.Lua.IIRacingReference>().InstancePerDependency();
            builder.RegisterType<Components.Playback.Lua.PlaybackInstanceThread>().As<Components.Playback.Lua.IPlaybackInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.Playback.Lua.PlaybackLuaReference>().As<Components.Playback.Lua.IPlaybackLuaReference>().InstancePerDependency();
            builder.RegisterType<Components.Twitch.Lua.TwitchLuaInstanceThread>().As<Components.Twitch.Lua.ITwitchLuaInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.Twitch.Lua.TwitchLuaReference>().As<Components.Twitch.Lua.ITwitchLuaReference>().InstancePerDependency();
            builder.RegisterType<Components.WinFormUI.Lua.WinFormInstanceThread>().As<Components.WinFormUI.Lua.IWinFormInstanceThread>().InstancePerDependency();
            builder.RegisterType<Components.WinFormUI.Lua.WinFormReference>().As<Components.WinFormUI.Lua.IWinFormReference>().InstancePerDependency();
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