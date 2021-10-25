using Models.Enums;
using System;

namespace Common.API.Models.DTO
{
    public interface IStationStateDTO 
    {
        public Guid? FlightId { get; set; }
        public FlightDirection? Direction { get; set; }
        public string Name { get; set; }
        public int? NewStationId { get; set; }
    }
}
