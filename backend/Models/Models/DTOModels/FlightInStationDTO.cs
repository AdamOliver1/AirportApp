using Common.Interfaces.Models.DTO;
using Models.Enums;
using System;

namespace Common.Models.DTOModels
{
    public class FlightInStationDTO : IFlightInStationDTO
    {
        public  Guid? FlightId { get; set; }
        public FlightDirection Direction { get; set; }
        public string Name { get; set; }
        public int? NewStationId { get; set; }
        public int? OldStationId { get; set; }
              
    }
}
