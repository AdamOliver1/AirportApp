using Models;
using System.Collections.Generic;

namespace Common.Extentions
{
    public interface IBuilder
    {
        /// <summary>
        /// Builds the station services
        /// </summary>
        /// <param name="stations">The stations that in the airport</param>
        /// <param name="stationRelations">The relations between the stations</param>
        /// <returns></returns>
        IStationServicesData BuildStations(IEnumerable<Station> stations, IEnumerable<StationsRelation> stationRelations);
    }
}
