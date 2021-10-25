using Common.Interfaces;
using System.Collections.Generic;

namespace Common.Extentions
{
    /// <summary>
    /// All the station services Lists that are essential.
    /// </summary>
    public interface IStationServicesData
    {
        List<IStationService> LandingStations { get; set; }
        List<IStationService> TakeoffStations { get; set; }
        List<IStationService> AllStationServices { get; set; }
    }
}
