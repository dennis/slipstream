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
            Application.Run(serviceProvider.GetRequiredService<UI.MainWindow>());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<UI.MainWindow>();
        }
    }
}
