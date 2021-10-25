using Common.Models;
using Models;
using System.Threading.Tasks;

namespace Common.Interfaces.Loggers
{
    public interface IFlightStationMovementLogger
    {
        void SaveFlightStationData(FlightStationDataToLog flightStationLogger);

        Task UpdateStationState(StationState stationState, Flight flight);
    }
}
