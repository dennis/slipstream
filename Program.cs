using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Slipstream
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            string cwd = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\";
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
            services.AddScoped<Backend.IEngine, Backend.Engine>();
            services.AddScoped<Backend.Services.IStateService>(x => new Backend.Services.StateService(x.GetService<Shared.IEventBus>(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\Slipstream\state.txt"));
            services.AddTransient<Shared.IApplicationVersionService, Shared.ApplicationVersionService>();
        }
    }
}
