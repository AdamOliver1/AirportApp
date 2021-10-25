using Common.Models;
using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAirportRepository
    {
        IEnumerable<Flight> GetFlights();
        IEnumerable<Station> GetStations();
        IEnumerable<StationState> GetAirportStationsState();
        Task<Station> SaveNewStationAsync(Station station);
        IEnumerable<StationsRelation> GetStationsRelations();   
        Task<StationsRelation> AddStationsConnection(int fromId, int toId, FlightDirection direction);
        Task<Flight> AddNewFlight(Flight flight);
        Task SaveFlightStationDataAsync(FlightStationDataToLog data);
        Task SaveNewStationStateAsync(StationState stationState);
        Task UpdateStationStateAsync(StationState stationState, Flight flight);
        IEnumerable<FlightStationDataToLog> GetFlightStationLogger();
        public IEnumerable<FlightStationDataToLog> GetUnfinishedNewFlights();
    }
}
