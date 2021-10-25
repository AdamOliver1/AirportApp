using Common.API.Models.DTO;
using Common.Interfaces.Models.DTO;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDataService
    {
        Task AddNewFlight(Flight flight);
        IEnumerable<IStationDTO> GetStationsDTO();
        IEnumerable<StationsRelation> GetStationsRelations();
        IEnumerable<IStationStateDTO> GetUnfinishedFlights();
    }
}
