using FlightsSimulator.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace FlightsSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Provider { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddScoped<MainWindow>();
            services.AddScoped<SimulatorViewModel>();

            Provider = services.BuildServiceProvider();

            Provider.GetRequiredService<MainWindow>().Show();
        }
    }
}
