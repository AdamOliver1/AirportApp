using BL.Extentions;
using BL.Services.MultyEntryStationService;
using BL.Services.StationServices;
using BL.StationBinders;
using Common.Enums;
using Common.Extentions;
using Common.Interfaces;
using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BL.Builder
{
    public class StationBuilder : IBuilder
    {
        public IStationServicesData BuildStations(IEnumerable<Station> stations, IEnumerable<StationsRelation> stationsRelations)
        {
            List<IStationService> takeoffStations = new List<IStationService>(),
                landingStations = new List<IStationService>(),
                stationServies = new List<IStationService>();
            
            IStationService stationService;
            foreach (var station in stations)
            {
                stationService = CreateStationService(stationsRelations, station);
                stationServies.Add(stationService);
                AddToFirstStationsIfNeeded(stationsRelations, stationService, landingStations, takeoffStations);
            }

            foreach (var service in stationServies)
            {
                service.NextTakeoffStations = GetNextStations(stationServies, service, FlightDirection.Takeoff);
                service.NextLandingStations = GetNextStations(stationServies, service, FlightDirection.Landing);
            }

            return new StationServicesData
            {
                AllStationServices = stationServies,
                LandingStations = landingStations,
                TakeoffStations = takeoffStations,
            };
        }

        private IStationService CreateStationService(IEnumerable<StationsRelation> stationsRelations, Station station)
        {
            IStationService stationService;
            DirectionAllOptions leadingDirection;
            bool MaxOneDestination = CheckMaxOneDestination(stationsRelations, station);
            bool landingAndTakeoffConnected = CheckIfMultiStation(stationsRelations, station, out leadingDirection);

            if (MaxOneDestination && landingAndTakeoffConnected)
                stationService = new MultyEntryStationService(station, new OneOptionStationBinder(), leadingDirection);
            else if (!MaxOneDestination && landingAndTakeoffConnected)
                stationService = new MultyEntryStationService(station, new MultipleOptionStationBinder(), leadingDirection);
            else if (MaxOneDestination && !landingAndTakeoffConnected)
                stationService = new StationService(station, new OneOptionStationBinder());
            else
                stationService = new StationService(station, new MultipleOptionStationBinder());

            return stationService;
        }

        private IEnumerable<IStationService> GetNextStations(List<IStationService> stationServies, IStationService service, FlightDirection direction)
        {
            return service.Station.StationsRelations
                .Where(sr => sr.Direction == direction)
                .Select(sr => stationServies
                .FirstOrDefault(s => s.Station.Id == sr.ToStation.Id));
        }

        private bool CheckIfMultiStation(IEnumerable<StationsRelation> stationsRelations, Station station, out DirectionAllOptions leadingDirection)
        {
            leadingDirection = default;
            if (CheckConnectedLandingAndTakeoff(stationsRelations, station))
            {
                bool isLandingLeading = CheckIfLeadingDirection(stationsRelations, station, FlightDirection.Landing);
                bool isTakeoffLeading = CheckIfLeadingDirection(stationsRelations, station, FlightDirection.Takeoff);
                leadingDirection = GetLeadingDirection(isLandingLeading, isTakeoffLeading);
                return true;
            }
            return false;
        }

        private DirectionAllOptions GetLeadingDirection(bool isLandingLeading, bool isTakeoffLeading)
        {
            DirectionAllOptions leadingDirection;
            if (isLandingLeading && isTakeoffLeading) leadingDirection = DirectionAllOptions.LandingAndTakeoff;
            else if (isLandingLeading) leadingDirection = DirectionAllOptions.Landing;
            else if (isTakeoffLeading) leadingDirection = DirectionAllOptions.Takeoff;
            else leadingDirection = DirectionAllOptions.None;
            return leadingDirection;
        }

        private bool CheckConnectedLandingAndTakeoff(IEnumerable<StationsRelation> stationsRelations, Station station)
        {
            var fromRelations = GetStationsIConnectedTo(stationsRelations, station);
            var toRelations = GetStationsConnectedToMe(stationsRelations, station);
            bool isLanding = false, isTakeoff = false;
            foreach (var relation in fromRelations)
            {
                if (relation.Direction == FlightDirection.Landing) isLanding = true;
                else isTakeoff = true;
            }
            foreach (var relation in toRelations)
            {
                if (relation.Direction == FlightDirection.Landing) isLanding = true;
                else isTakeoff = true;
            }
            return isLanding && isTakeoff;
        }

        private bool CheckIfLeadingDirection(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection direction)
        {
            List<bool> isAllPathsDirectToMultiConnectedStations = new List<bool>();
            CheckIsLeadingDirection(stationsRelations, station, direction, isAllPathsDirectToMultiConnectedStations);
            return isAllPathsDirectToMultiConnectedStations.All(b => b == true);
        }

        private void CheckIsLeadingDirection(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection direction, List<bool> isAllPathsDirectToMultiConnectedStations)
        {
            var relations = GetStationsIConnectedTo(stationsRelations, station, direction);
            if (relations.Count() == 0)
            {
                isAllPathsDirectToMultiConnectedStations.Add(false);
                return;
            }
            foreach (var relation in relations)
            {
                if (CheckConnectedLandingAndTakeoff(stationsRelations, relation.ToStation))
                {
                    isAllPathsDirectToMultiConnectedStations.Add(true);
                    return;
                }
                CheckIsLeadingDirection(stationsRelations, relation.ToStation, direction, isAllPathsDirectToMultiConnectedStations);
            }
        }

        private void AddToFirstStationsIfNeeded(IEnumerable<StationsRelation> stationsRelations, IStationService stationService, List<IStationService> landingStations, List<IStationService> takeoffStations)
        {
            if (GetNumOfStationsConnectedToMe(stationsRelations, stationService.Station, FlightDirection.Landing) == 0 &&
                   GetNumOfStationsIConnectedTo(stationsRelations, stationService.Station, FlightDirection.Landing) > 0)
                landingStations.Add(stationService);
            if (GetNumOfStationsConnectedToMe(stationsRelations, stationService.Station, FlightDirection.Takeoff) == 0 &&
                 GetNumOfStationsIConnectedTo(stationsRelations, stationService.Station, FlightDirection.Takeoff) > 0)
                takeoffStations.Add(stationService);
        }

        private bool CheckMaxOneDestination(IEnumerable<StationsRelation> stationsRelations, Station station)
        {
            return GetNumOfStationsIConnectedTo(stationsRelations, station, FlightDirection.Landing) <= 1 &&
                GetNumOfStationsIConnectedTo(stationsRelations, station, FlightDirection.Takeoff) <= 1;
        }

        private IEnumerable<StationsRelation> GetStationsIConnectedTo(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection? direction = null)
        {
            return direction == null ? stationsRelations.Where(sr => sr.FromStation == station) :
           stationsRelations.Where(sr => sr.FromStation == station && sr.Direction == direction);         
        }

        private IEnumerable<StationsRelation> GetStationsConnectedToMe(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection? direction = null)
        {
            return direction == null ? stationsRelations.Where(sr => sr.ToStation == station) :
             stationsRelations.Where(sr => sr.ToStation == station && sr.Direction == direction);          
        }

        private int GetNumOfStationsConnectedToMe(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection direction)
        {
            return GetStationsConnectedToMe(stationsRelations, station, direction).Count();
        }
        
        private int GetNumOfStationsIConnectedTo(IEnumerable<StationsRelation> stationsRelations, Station station, FlightDirection direction)
        {
            return GetStationsIConnectedTo(stationsRelations, station, direction).Count();
        }
    }
}
