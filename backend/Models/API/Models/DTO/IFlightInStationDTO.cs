using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces.Models.DTO
{
    public interface IFlightInStationDTO
    {
        public Guid? FlightId { get; set; }
        public FlightDirection Direction { get; set; }
        public string Name { get; set; }
        public int? NewStationId { get; set; }
        public int? OldStationId { get; set; }
    }
}
