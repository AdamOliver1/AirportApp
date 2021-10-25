using BL.Services.MultyEntryStationService;
using Common.Enums;
using Common.Interfaces;
using Common.Interfaces.StationBinders;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.StationBinders
{

    public class MultipleOptionStationBinder : IStationsBinder
    {
        public MultipleOptionStationBinder() { }


        public IStationService GetNextStation(FlightDirection direction, IEnumerable<IStationService> connectedStations)
        {
            if (connectedStations == null) return null;
            return connectedStations.Count() <= 1 ?
               connectedStations.FirstOrDefault() :
               FindBestOptionForMultiConnectedStation(direction, connectedStations);
        }

        #region Finders

        private IStationService FindBestOptionForMultiConnectedStation(FlightDirection direction, IEnumerable<IStationService> connectedStations)
        {
            if (connectedStations.All(s => s.CurrentFlightService == null))
                return FindBestStationByTime(connectedStations);
            if (connectedStations.All(s => s is MultyEntryStationService))
                return FindBestForMultiEntryStations(direction, connectedStations);
            if (connectedStations.Any(s => s is MultyEntryStationService))
                return FindBestForSomeAreMultyEntryStations(direction, connectedStations);
            return FindBestStationByTime(connectedStations);
        }
        private IStationService FindBestStationByTime(IEnumerable<IStationService> connectedStations)
        {
            IStationService resultEmpty = null, resultNotEmpty = null;
            TimeSpan minTimeEmpty = TimeSpan.MaxValue;
            double minTimeNotEmpty = double.MaxValue;
            foreach (var stationService in connectedStations)
            {
                if (CheckEmptyStation(minTimeEmpty, stationService))
                    UpdateResultStation(out resultEmpty, out minTimeEmpty, stationService);
                else if (stationService.TotalWaitingTime < minTimeNotEmpty)
                    UpdateResultStation(out resultNotEmpty, out minTimeNotEmpty, stationService);
            }
            return resultEmpty != null ? resultEmpty : resultNotEmpty;
        }

        private IStationService FindBestForMultiEntryStations(FlightDirection direction, IEnumerable<IStationService> connectedStations)
        {
            bool isLanding = false, isTakeoff = false;
            foreach (var stationService in connectedStations)
            {
                if ((stationService as MultyEntryStationService).LeadingDirectaion == DirectionAllOptions.Landing)
                    isLanding = true;
                else if ((stationService as MultyEntryStationService).LeadingDirectaion == DirectionAllOptions.Takeoff)
                    isTakeoff = true;
            }
            if (direction == FlightDirection.Takeoff && isTakeoff)
                return FindBestOptionForLeadingDirection(connectedStations, FlightDirection.Takeoff);
            if (direction == FlightDirection.Landing && isLanding)
                return FindBestOptionForLeadingDirection(connectedStations, FlightDirection.Landing);
            return FindBestStationByTime(connectedStations);
        }

        private IStationService FindBestForSomeAreMultyEntryStations(FlightDirection direction, IEnumerable<IStationService> connectedStations)
        {
            var newConnected = new List<IStationService>();
            foreach (var stationService in connectedStations)
            {
                if (stationService is not MultyEntryStationService)
                    newConnected.Add(stationService);
            }
            return FindBestStationByTime(newConnected);
        }



        private IStationService FindBestOptionForLeadingDirection(IEnumerable<IStationService> connectedStations, FlightDirection LeadingDirection)
        {
            IStationService resultTakeoff = null, resultLanding = null, resultAll = null, resultEmpty = null;
            double minWaitingTimeTakeoffStations = double.MaxValue, minWaitingTimeLandingStations = double.MaxValue, minWaitingTimeAllBoth = double.MaxValue;
            TimeSpan minWaitingTimeEmpty = TimeSpan.MaxValue;
            foreach (var station in connectedStations)
            {
                if (CheckEmptyStation(minWaitingTimeEmpty, station))
                    UpdateResultStation(out resultEmpty, out minWaitingTimeEmpty, station);
                else if (CheckWaitingFlightsDirection(minWaitingTimeTakeoffStations, station, DirectionAllOptions.Takeoff))
                    UpdateResultStation(out resultTakeoff, out minWaitingTimeTakeoffStations, station);
                else if (CheckWaitingFlightsDirection(minWaitingTimeLandingStations, station, DirectionAllOptions.Landing))
                    UpdateResultStation(out resultLanding, out minWaitingTimeLandingStations, station);
                else if (CheckWaitingFlightsDirection(minWaitingTimeAllBoth, station, DirectionAllOptions.LandingAndTakeoff))
                    UpdateResultStation(out resultAll, out minWaitingTimeAllBoth, station);
            }
            return SortPriorities(resultTakeoff, resultLanding, resultAll, resultEmpty, LeadingDirection);
        }

        private IStationService SortPriorities(IStationService resultTakeoff, IStationService resultLanding, IStationService resultAll, IStationService resultEmpty, FlightDirection LeadingDirection)
        {
            if (LeadingDirection == FlightDirection.Takeoff && resultTakeoff != null) return resultTakeoff;
            if (LeadingDirection == FlightDirection.Landing && resultLanding != null) return resultLanding;

            if (resultAll != null) return resultAll;
            if (resultEmpty != null) return resultEmpty;

            if (LeadingDirection == FlightDirection.Takeoff && resultLanding != null) return resultLanding;
            return resultTakeoff;
        }

        #endregion

        #region UpdateResultStation

        private void UpdateResultStation(out IStationService toBeUpdated, out double updatedTime, IStationService station)
        {
            toBeUpdated = station;
            updatedTime = station.TotalWaitingTime;
        }

        private void UpdateResultStation(out IStationService toBeUpdated, out TimeSpan updatedTime, IStationService station)
        {
            toBeUpdated = station;
            updatedTime = station.WaitingTime;
        }
        #endregion

        #region Checks
        private bool CheckWaitingFlightsDirection(double minWaitingTime, IStationService station, DirectionAllOptions direction)
        {
            return station.WaitingFlightsDirection == direction && station.TotalWaitingTime < minWaitingTime;
        }

        private bool CheckEmptyStation(TimeSpan minWaitingTimeEmpty, IStationService station)
        {
            return station.CurrentFlightService == null && station.WaitingTime < minWaitingTimeEmpty;
        }

        #endregion
    }
}
