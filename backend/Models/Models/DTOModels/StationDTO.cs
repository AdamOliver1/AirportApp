using Common.Interfaces.Models.DTO;

namespace Common.Models.DTOModels
{
    public class StationDTO : IStationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
