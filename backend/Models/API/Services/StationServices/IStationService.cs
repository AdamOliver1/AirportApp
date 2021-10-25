using Models.Enums;
using System;
using System.Collections.Generic;

namespace Common.Interfaces
{

    public interface IStationService : IFlightMovable, IStationInformation
    {
        public TimeSpan WaitingTime { get; }
        public IEnumerable<IStationService> NextTakeoffStations { get; set; }
        public IEnumerable<IStationService> NextLandingStations { get; set; }
        public IStationService GetNextStation(FlightDirection direction);
    }
   
}
