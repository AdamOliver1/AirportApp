using Common.Models;
using Models;
using System.Collections.Generic;

namespace Common.Interfaces
{
    /// <summary>
    /// The class who is responsible for the logic of the airport
    /// </summary>
    public interface IAirportLogic
    {      
        bool DoStationsNeedToInit { get; }
        void RegisterFlight(IFlightService flightService);
        void InitStations(IEnumerable<Station> stations, IEnumerable<StationsRelation> stationRelations);

        /// <summary>
        /// Gets the states of the stations and orders the unfinished flights befoer the server disconnected, into there stations.
        /// </summary>
        /// <param name="stationsState">
        /// The StationStates of all the stations, determines if a station has an unfinised flight.
        /// </param>
        void SetUnfinishedFlights(IEnumerable<StationState> stationsState);

        /// <summary>
        /// Registers new flights that didn't have time to enter a station before the server disconnected
        /// </summary>
        /// <param name="stationsState">
        /// The StationStates of all the stations, determines if a station has an unfinised flight.
        /// </param>
        void RegisterUnfinishedNewFlights(IEnumerable<FlightStationDataToLog> flightStationLogger);
    }
}
