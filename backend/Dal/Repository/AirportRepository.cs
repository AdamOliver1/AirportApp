using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Server.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class AirportRepository : IAirportRepository
    {
        AirportContext _dataContext;
        public AirportRepository(AirportContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<Flight> GetFlights()
        {
            try
            {
                return _dataContext.Flights;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }

        }

        public IEnumerable<Station> GetStations()
        {
            try
            {
                return _dataContext.Stations.Include(s => s.StationState).Include(s => s.StationsRelations);
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        public IEnumerable<FlightStationDataToLog> GetFlightStationLogger()
        {
            try
            {
                return _dataContext.FlightStationLogger;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }

        }

        public IEnumerable<StationState> GetAirportStationsState()
        {
            try
            {
                return _dataContext.AirportStationsState.Include(s => s.Station).Include(s => s.Flight);
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        public IEnumerable<StationsRelation> GetStationsRelations()
        {
            try
            {
                return _dataContext.StationsRelation;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        async public Task<Station> SaveNewStationAsync(Station station)
        {
            try
            {
                _dataContext.Stations.Add(station);
                await _dataContext.SaveChangesAsync();
                return _dataContext.Stations.FirstOrDefault(s => s.Id == station.Id);
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        async public Task SaveNewStationStateAsync(StationState stationState)
        {
            try
            {
                _dataContext.AirportStationsState.Add(stationState);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
            }
        }

        public async Task SaveFlightStationDataAsync(FlightStationDataToLog data)
        {
            _dataContext.FlightStationLogger.Add(data);
            await _dataContext.SaveChangesAsync();
            try
            {
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
            }
        }

        async public Task UpdateStationStateAsync(StationState stationState, Flight flight)
        {
            var state = _dataContext.AirportStationsState.Include(s => s.Flight).SingleOrDefault(s => s.StationId == stationState.StationId);
            state.Flight = null;
            state.FlightId = flight?.Id;

            await _dataContext.SaveChangesAsync();
            try
            {
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
            }

        }

        async public Task<StationsRelation> AddStationsConnection(int fromId, int toId, FlightDirection direction)
        {
            try
            {
                var relation = new StationsRelation
                {
                    FromStationId = fromId,
                    ToStationId = toId,
                    Direction = direction
                };
                _dataContext.StationsRelation.Add(relation);
                await _dataContext.SaveChangesAsync();
                return relation;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        async public Task<Flight> AddNewFlight(Flight flight)
        {
            try
            {
                flight.Id = Guid.NewGuid();
                _dataContext.Flights.Add(flight);
                await _dataContext.SaveChangesAsync();
                return flight;
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }
        }

        public IEnumerable<FlightStationDataToLog> GetUnfinishedNewFlights()
        {
            try
            {
                return _dataContext.FlightStationLogger
                .Where(fl1 => fl1.ToStation == null && fl1.FromStation == null && _dataContext.FlightStationLogger.Where(fl2 => fl1.FlightId == fl2.FlightId).Count() == 1).Include(f => f.Flight).OrderBy(f => f.EntranceStaionDate);              
            }
            catch (Exception ex)
            {
                ex.LoggExceptionToText();
                return null;
            }

        }

    }
}
