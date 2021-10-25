using Models;
using Models.Enums;

namespace Common.Interfaces
{
    public interface IFlightService
    {
        public FlightDirection Direction { get; }
        public IStationService CurrentStation { get; set; }
        public Flight Flight { get; set; }

    }
}
