using Common.API.Models.DTO;
using Models.Enums;
using System;

namespace Common.Models.DTOModels
{
    public class StationStateDTO : IStationStateDTO
    {
        public Guid? FlightId { get; set; }
        public FlightDirection? Direction { get; set; }
        public string Name { get; set; }
        public int? NewStationId { get; set; }
    }
}
