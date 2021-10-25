using Common.Interfaces;
using Models;
using Models.Enums;

namespace BL.Services
{
    public class FlightService : IFlightService
    {       
        public FlightService(Flight flight)
        {                   
            Flight = flight;
            CurrentStation = null;
        }
        public IStationService CurrentStation { get; set; }
        public Flight Flight { get; set; }

        public FlightDirection Direction => Flight.Direction;      
    }
}
