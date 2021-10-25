using Common.Interfaces.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs.HubTypes
{
    public interface IAirportClient
    {
        Task RecieveFlightMovement(IFlightInStationDTO flightInStationDTO);
        Task RecieveNewFlight(INewFlightDTO newFlightDTO);
        Task RecieveAirportStations(List<IStationDTO> listOfStations);
    }
}
