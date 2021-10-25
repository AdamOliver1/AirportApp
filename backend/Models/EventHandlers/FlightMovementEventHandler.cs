using Common.Interfaces;
using System;

namespace Common.EventHandlers
{
    public delegate void FlightMovementEventHandler(object sender, FlightMovementEventArgs e);
    public class FlightMovementEventArgs : EventArgs
    {

        public IFlightService FlightService { get; }
        public IStationService NewStationService { get; set; }
       

        public FlightMovementEventArgs(IFlightService flightService, IStationService newStationService)
        {     
            NewStationService = newStationService;
            FlightService = flightService;           
        }
    }
}
