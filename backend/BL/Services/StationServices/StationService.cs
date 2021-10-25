using BL.Extentions;
using Common.API.Extensions;
using Common.Enums;
using Common.EventHandlers;
using Common.Interfaces;
using Common.Interfaces.StationBinders;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BL.Services.StationServices
{
    public class StationService : IStationService
    {
        #region Data Members
        ICountdown _countdown;
        Semaphore _gate;
        protected IFlightService _currentFlightService = null;
        protected Queue<IFlightService> _waitingFlights;
        int _numOfFlights;
        object _lock = new object();
        private readonly IStationsBinder _stationBinder;

        #endregion

        #region Public Properties
        public IFlightService CurrentFlightService => _currentFlightService;
        public TimeSpan WaitingTime => Station.WaitingTime;
        public Station Station { get; }

        public bool IsOccupied => _numOfFlights > 0;
        public double TotalWaitingTime => GetTotalWaitingTime();

        public event FlightMovementEventHandler NotifyFlightMovementEvent;
        public IEnumerable<IStationService> NextTakeoffStations { get; set; }
        public IEnumerable<IStationService> NextLandingStations { get; set; }
        public DirectionAllOptions WaitingFlightsDirection { get { return GetWaitingFlightsDirection(); } }
        #endregion


        public StationService(Station station, IStationsBinder stationBinder)
        {
            _gate = new Semaphore(1, 1);
            Station = station;
            _stationBinder = stationBinder;
            _waitingFlights = new Queue<IFlightService>();
            _countdown = new Countdown();
        }

        public void ExitStation()
        {
            lock (_lock)
            {
                _countdown.Reset();
                _numOfFlights--;             
                UpdateEnteringStation();
                _gate.Release();
            }


        }

        public IFlightService RegisterFlightToStation(IFlightService flightService)
        {
            AddFlightToQueueIfNeeded(flightService);
            _gate.WaitOne();
            _currentFlightService = _currentFlightService == null ? flightService : _currentFlightService;
            NotifyFlightMovement();
            return EnterStation();
        }

        public virtual IStationService GetNextStation(FlightDirection direction)
        {
            lock (_lock)
            {              
                return _stationBinder.GetNextStation(direction,direction == FlightDirection.Landing ? NextLandingStations : NextTakeoffStations
                    );
            }
        }

        protected virtual void UpdateEnteringStation()
        {
            if (_waitingFlights.Count > 0)
                _currentFlightService = _waitingFlights.Dequeue();
            else
                _currentFlightService = null;
        }

        protected IFlightService EnterStation()
        {

            _currentFlightService.CurrentStation?.ExitStation();
            _countdown.StartCountDown(Station.WaitingTime);        
            Thread.Sleep(Station.WaitingTime);
            _countdown.Stop();        
            return _currentFlightService;
        }

        private double GetTotalWaitingTime()
        {
            lock (_lock)
                return _countdown.ElapsedMilliseconds + (_waitingFlights.Count * Station.WaitingTime.TotalMilliseconds);
        }

        DirectionAllOptions GetWaitingFlightsDirection()
        {
            lock (_lock)
            {
                if (_waitingFlights.Count == 0)
                    return _currentFlightService != null ? _currentFlightService.Direction.Convert() : DirectionAllOptions.None;
                bool isLanding = false, isTakeoff = false;
                foreach (var flight in _waitingFlights)
                {
                    if (flight.Direction == FlightDirection.Landing)
                        isLanding = true;
                    else
                        isTakeoff = true;
                }

                if (isLanding && isTakeoff) return DirectionAllOptions.LandingAndTakeoff;
                if (isLanding) return DirectionAllOptions.Landing;
                if (isTakeoff) return DirectionAllOptions.Takeoff;
                return DirectionAllOptions.None;
            }

        }


        private void NotifyFlightMovement()
        {
            lock (_lock)
            {             
                NotifyFlightMovementEvent?.Invoke(this, new FlightMovementEventArgs(_currentFlightService, this));
            }
        }


        protected void AddFlightToQueueIfNeeded(IFlightService flightService)
        {
            lock (_lock)
            {
                ++_numOfFlights;
                if (_currentFlightService != null)
                    _waitingFlights.Enqueue(flightService);
            }
        }

    }
}
