using BL.Services.StationServices;
using Common.API.Services.StationServices;
using Common.Enums;
using Common.Interfaces.StationBinders;
using Models;

namespace BL.Services.MultyEntryStationService
{
    public class MultyEntryStationService : StationService, IMultyEntryStationService
    {    
        public DirectionAllOptions LeadingDirectaion { get; }
        public MultyEntryStationService(Station station, IStationsBinder stationBinder, DirectionAllOptions leadingDirectaion = DirectionAllOptions.None) :base(station, stationBinder)
        {
            LeadingDirectaion = leadingDirectaion;
        }
      
    }
}
