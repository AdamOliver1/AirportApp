using Common.Interfaces.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces.Hubs
{
    /// <summary>
    /// The hub that the client connects to
    /// </summary>
    public interface IFlightHub 
    {
        public Task NotifiyNewFlight(INewFlightDTO newFlightDTO);        
        Task NotifyFlightMovemont(IFlightInStationDTO flightInStationDTO);
        Task NotifyAirportStations(List<IStationDTO> listOfStations);
    }
}
