using Common.Interfaces;
using Common.Interfaces.Loggers;
using Common.Models;
using Dal.Repository;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Server.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Loggers
{
    public class FlightStationMovementLogger : IFlightStationMovementLogger
    {
        private readonly IServiceProvider _provider;
        public FlightStationMovementLogger(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async void SaveFlightStationData(FlightStationDataToLog flightStationLogger)
        {
            try
            {
                IAirportRepository airportRepository = _provider.CreateScope().ServiceProvider.GetRequiredService<IAirportRepository>();
                await airportRepository.SaveFlightStationDataAsync(flightStationLogger);
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
            }      
        }      

        public async Task UpdateStationState(StationState stationState, Flight flight)
        {
            try
            {
                IAirportRepository airportRepository = _provider.CreateScope().ServiceProvider.GetRequiredService<IAirportRepository>();
                await airportRepository.UpdateStationStateAsync(stationState, flight);
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();               
            }        
        }
    }
}
