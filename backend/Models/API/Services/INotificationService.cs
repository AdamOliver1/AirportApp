using Common.EventHandlers;

namespace Common.Interfaces
{
    public interface INotificationService
    {
        void NotifyFlightMovemont(object sender, FlightMovementEventArgs e);
        void NotifiyNewFlight(object sender, NewFlightEventArgs e);
    }
}
