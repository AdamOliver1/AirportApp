using Common.Interfaces;
using Common.Interfaces.StationBinders;
using Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BL.StationBinders
{
    public class OneOptionStationBinder : IStationsBinder
    {
        public OneOptionStationBinder() { }
        public IStationService GetNextStation(FlightDirection direction, IEnumerable<IStationService> connectedStations)
        {           
            return connectedStations?.FirstOrDefault();
        }
    }
}
