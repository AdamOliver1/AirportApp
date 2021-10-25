using Models.Enums;
using System.Collections.Generic;

namespace Common.Interfaces.StationBinders
{
    public interface IStationsBinder
    {
        /// <summary>
        /// Gets the next station between the connected Stations
        /// </summary>
        /// <param name="direction">
        /// What direction the next stations needs to be able to connect
        /// </param>
        /// <param name="connectedStations">
        /// the stations that are optional to be the next one.
        /// </param>
        /// <returns></returns>
        IStationService GetNextStation(FlightDirection direction, IEnumerable<IStationService> connectedStations);
    }
}
