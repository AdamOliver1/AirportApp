namespace Common.Interfaces
{
    public interface IFlightMovable
    {
        void ExitStation();
        IFlightService RegisterFlightToStation(IFlightService flightService);      
    }
}
