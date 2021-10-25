using Common.EventHandlers;
using Common.Interfaces;
using Common.Interfaces.Hubs;
using Common.Interfaces.Loggers;
using Common.Models.DTOModels;
using Models;
using System;


namespace BL.Services
{
    public class NotificationService : INotificationService
    {
        readonly IFlightHub _flightHub;
        readonly IFlightStationMovementLogger _flightStationMovementLogger;

        public NotificationService(IFlightHub flightHub, IFlightStationMovementLogger flightStationMovementLogger)
        {
            _flightHub = flightHub;
            _flightStationMovementLogger = flightStationMovementLogger;
        }

        public async void NotifyFlightMovemont(object sender, FlightMovementEventArgs e)
        {
            SaveToFlightStationLogger(e.FlightService, e.NewStationService);
            UpdateStationState(e);

            await _flightHub.NotifyFlightMovemont(new FlightInStationDTO
            {
                FlightId = e.FlightService.Flight.Id,
                Direction = e.FlightService.Flight.Direction,
                Name = e.FlightService.Flight.Name,
                NewStationId = e.NewStationService == null ? null : e.NewStationService.Station.Id,
                OldStationId = e.FlightService.CurrentStation == null ? null : e.FlightService.CurrentStation.Station.Id
            });
        }

        async private void UpdateStationState(FlightMovementEventArgs e)
        {
            var current = e.FlightService.CurrentStation?.Station.StationState;
            var next = e.NewStationService?.Station.StationState;
            if (current != null)
                await _flightStationMovementLogger.UpdateStationState(current, null);
            if (next != null)
                await _flightStationMovementLogger.UpdateStationState(next, e.FlightService.Flight);
        }

        private void SaveToFlightStationLogger(IFlightService flightService, IStationService newStationService)
        {
            var flightStationData = new FlightStationDataToLog
            {
                FlightId = flightService.Flight.Id,
                Direction = flightService.Direction,
                FromStationId = flightService.CurrentStation == null ? null : flightService.CurrentStation.Station.Id,
                ToStationId = newStationService == null ? null : newStationService.Station.Id,
                EntranceStaionDate = DateTime.Now
            };
            _flightStationMovementLogger.SaveFlightStationData(flightStationData);
        }

        public async void NotifiyNewFlight(object sender, NewFlightEventArgs e)
        {
            if(e.ToSave)
                SaveToFlightStationLogger(e.FlightService, null);     
            
            await _flightHub.NotifiyNewFlight(
                new NewFlightDTO
                {
                    Id = e.FlightService.Flight.Id,
                    Direction = e.FlightService.Direction,
                    Name = e.FlightService.Flight.Name,
                    StartDate = e.FlightService.Flight.Date,
                });
        }
    }
}
