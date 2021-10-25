using Common.Extentions;
using Common.Interfaces;
using System.Collections.Generic;

namespace BL.Extentions
{
    public class StationServicesData : IStationServicesData
    {
        public List<IStationService> AllStationServices { get; set; }
        public List<IStationService> LandingStations { get; set; }
        public List<IStationService> TakeoffStations { get; set; }           
    }
}
