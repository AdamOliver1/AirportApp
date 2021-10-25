using Models;
using System;

namespace Common.Models
{
    public class StationState
    {
        public int StationId { get; set; }
        public Station Station { get; set; }
        public Guid? FlightId { get; set; }
        public Flight Flight { get; set; }
    }
}
