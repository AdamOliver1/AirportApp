using Common.Enums;
using Common.Interfaces;

namespace Common.API.Services.StationServices
{
    /// <summary>
    ///   A station that at list to stations connected to him with both landing and takeoff    
    /// </summary>
    public interface IMultyEntryStationService : IStationService
    {
        /// <summary>
        /// The direction that can couse all the flights to get stucked
        /// </summary>
        public DirectionAllOptions LeadingDirectaion { get; }
    }
}
