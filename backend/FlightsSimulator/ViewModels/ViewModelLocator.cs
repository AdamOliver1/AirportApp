using Microsoft.Extensions.DependencyInjection;
namespace FlightsSimulator.ViewModels
{
    public class ViewModelLocator
    {
        public SimulatorViewModel Simulator => App.Provider.GetRequiredService<SimulatorViewModel>();
    }
}
