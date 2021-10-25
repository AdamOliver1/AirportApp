using Common.Enums;
using Common.EventHandlers;
using Models;

namespace Common.Interfaces
{
    public interface IStationInformation
    {
        event FlightMovementEventHandler NotifyFlightMovementEvent;
         Station Station { get; }
        bool IsOccupied { get; }       
        double TotalWaitingTime { get; }
        DirectionAllOptions WaitingFlightsDirection { get; }
        IFlightService CurrentFlightService { get; }
         
    }
}
