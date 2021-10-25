using Models.Enums;
using System;

namespace Common.Interfaces.Models.DTO
{
    public interface INewFlightDTO 
    {       
        public FlightDirection Direction { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
