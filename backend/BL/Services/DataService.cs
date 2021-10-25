using Common.API.Models.DTO;
using Common.Interfaces;
using Common.Interfaces.Models.DTO;
using Common.Models;
using Common.Models.DTOModels;
using Models;
using Models.Enums;
using Server.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services
{
    public class DataService : IDataService
    {
        IAirportRepository _airportRepository;
        IAirportLogic _airportLogic;     
        public DataService(IAirportRepository airportRepository, IAirportLogic airportLogic)
        {
            _airportRepository = airportRepository;
            _airportLogic = airportLogic;       
        }

        void CreateDataIfEmpty()
        {
            if (_airportRepository.GetStations().Count() == 0)
            {             
                CreateData();
            }
            if (_airportLogic.DoStationsNeedToInit)
                _airportLogic.InitStations(_airportRepository.GetStations(), _airportRepository.GetStationsRelations());

        }

        private async void CreateData()
        {
            var station1 = await _airportRepository.SaveNewStationAsync(new Station { Name = "1", WaitingTime = new TimeSpan(0, 0, 1) });
            var station2 = await _airportRepository.SaveNewStationAsync(new Station { Name = "2", WaitingTime = new TimeSpan(0, 0, 2) });
            var station3 = await _airportRepository.SaveNewStationAsync(new Station { Name = "3", WaitingTime = new TimeSpan(0, 0, 1) });
            var station4 = await _airportRepository.SaveNewStationAsync(new Station { Name = "4", WaitingTime = new TimeSpan(0, 0, 2) });
            var station5 = await _airportRepository.SaveNewStationAsync(new Station { Name = "5", WaitingTime = new TimeSpan(0, 0, 3) });
            var station6 = await _airportRepository.SaveNewStationAsync(new Station { Name = "6", WaitingTime = new TimeSpan(0, 0, 2) });
            var station7 = await _airportRepository.SaveNewStationAsync(new Station { Name = "7", WaitingTime = new TimeSpan(0, 0, 1) });
            var station8 = await _airportRepository.SaveNewStationAsync(new Station { Name = "8", WaitingTime = new TimeSpan(0, 0, 2) });

            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station1 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station2 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station3 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station4 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station5 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station6 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station7 });
            await _airportRepository.SaveNewStationStateAsync(new StationState { Station = station8 });

            await _airportRepository.AddStationsConnection(station1.Id, station2.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station2.Id, station3.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station3.Id, station4.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station4.Id, station5.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station5.Id, station6.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station5.Id, station7.Id, FlightDirection.Landing);
            await _airportRepository.AddStationsConnection(station6.Id, station8.Id, FlightDirection.Takeoff);
            await _airportRepository.AddStationsConnection(station7.Id, station8.Id, FlightDirection.Takeoff);
            await _airportRepository.AddStationsConnection(station8.Id, station4.Id, FlightDirection.Takeoff);
        }

        async public Task AddNewFlight(Flight flight)
        {
            try
            {
                await _airportRepository.AddNewFlight(flight);
                _airportLogic.RegisterFlight(new FlightService(flight));
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
            }
        }

        public IEnumerable<StationsRelation> GetStationsRelations()
        {
            return _airportRepository.GetStationsRelations();
        }

        public IEnumerable<IStationDTO> GetStationsDTO()
        {
            try
            {
                CreateDataIfEmpty();
                var listOfStations = new List<IStationDTO>();
                foreach (var station in _airportRepository.GetStations())
                    listOfStations.Add(new StationDTO { Id = station.Id, Name = station.Name });
                return listOfStations;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        public IEnumerable<IStationStateDTO> GetUnfinishedFlights()
        {
            try
            {
                var listOfStations = new List<IStationStateDTO>();
                var stationsState = _airportRepository.GetAirportStationsState();
                foreach (var stationState in stationsState)
                {
                    listOfStations.Add(
                       new StationStateDTO
                       {
                           FlightId = stationState.FlightId,
                           Direction = stationState.Flight?.Direction,
                           Name = stationState.Flight?.Name,
                           NewStationId = stationState.StationId
                       });
                }
                _airportLogic.RegisterUnfinishedNewFlights(_airportRepository.GetUnfinishedNewFlights());
                _airportLogic.SetUnfinishedFlights(stationsState);
                return listOfStations;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }
    }
}
