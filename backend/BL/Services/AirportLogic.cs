using BL.Services;
using BL.StationBinders;
using Common.EventHandlers;
using Common.Extentions;
using Common.Interfaces;
using Common.Interfaces.StationBinders;
using Common.Models;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    public class AirportLogic : IAirportLogic
    {
        readonly IStationsBinder _stationBinder;
        IEnumerable<IStationService> _stationServices;
        readonly INotificationService _notificationService;
        readonly IBuilder _stationBuilder;
        List<IStationService> FirstLandingStations, FirstTakeOffStations;
        object _lock = new object();
        bool _doStationsNeedToInit = true;
        public bool DoStationsNeedToInit { get => _doStationsNeedToInit; }
        public event NewFlightEventHandler NotifyNewFlight;

        public AirportLogic(INotificationService notificationService, IBuilder stationBuilder)
        {
            _stationBinder = new MultipleOptionStationBinder();
            _stationBuilder = stationBuilder;
            _notificationService = notificationService;
            NotifyNewFlight += _notificationService.NotifiyNewFlight;
            InitProperties();
        }

        private void InitProperties()
        {
            FirstLandingStations = new List<IStationService>();
            FirstTakeOffStations = new List<IStationService>();
        }

        public void InitStations(IEnumerable<Station> stations, IEnumerable<StationsRelation> stationRelations)
        {
            _doStationsNeedToInit = false;
            var stationsData = _stationBuilder.BuildStations(stations, stationRelations);
            _stationServices = stationsData.AllStationServices;
            FirstLandingStations = stationsData.LandingStations;
            FirstTakeOffStations = stationsData.TakeoffStations;

            foreach (var service in _stationServices)
                service.NotifyFlightMovementEvent += _notificationService.NotifyFlightMovemont;
        }
        public void RegisterFlight(IFlightService flightService)
        {
            StartFlightProcess(flightService);
        }
        private void StartFlightProcess(IFlightService flightService, bool saveFlight = true)
        {
            NotifyNewFlight(this, new NewFlightEventArgs(flightService, saveFlight));
            Task.Run(() =>
            {
                DelayFlightIfNeeded(flightService);
                IStationService firstStation = GetFirstStation(flightService.Direction);
                FlightTransformationBetweenStations(ref flightService, ref firstStation);
                EndFlight(flightService);          
            });
        }

        private void FlightTransformationBetweenStations(ref IFlightService flightService, ref IStationService nextStation)
        {
            while (nextStation != null)
            {
                flightService = nextStation.RegisterFlightToStation(flightService);
                flightService.CurrentStation = nextStation;
                nextStation = nextStation.GetNextStation(flightService.Direction);
            }
        }

        private void EndFlight(IFlightService flightService)
        {
            lock (_lock)
            {
                _notificationService.NotifyFlightMovemont(flightService.CurrentStation, new FlightMovementEventArgs(flightService, null));
                flightService.CurrentStation?.ExitStation();
            }
        }

        private void DelayFlightIfNeeded(IFlightService flightService)
        {
            if (flightService.Flight.Date > DateTime.Now)
                Thread.Sleep(flightService.Flight.Date - DateTime.Now);
        }

        private IStationService GetFirstStation(FlightDirection direction)
        {
            return _stationBinder.GetNextStation(direction, direction == FlightDirection.Landing ? FirstLandingStations : FirstTakeOffStations);
        }

        public void SetUnfinishedFlights(IEnumerable<StationState> stationsState)
        {
            if (_stationServices == null) return;
            foreach (var station in stationsState.Where(s => s.Flight != null))
            {
                Task.Run(() =>
                {
                    IFlightService flightService = new FlightService(station.Flight);
                    IStationService nextStation = _stationServices.SingleOrDefault(s => s.Station.Id == station.StationId);
                    FlightTransformationBetweenStations(ref flightService, ref nextStation);
                    EndFlight(flightService);
                });
            }

        }

        public void RegisterUnfinishedNewFlights(IEnumerable<FlightStationDataToLog> flightStationLogger)
        {
            foreach (var fl in flightStationLogger)
                StartFlightProcess(new FlightService(fl.Flight), false);
        }
    }
}

