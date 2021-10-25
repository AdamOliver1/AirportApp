using Common.Interfaces.Models.DTO;
using Models.Enums;
using System;

namespace Common.Models.DTOModels
{
    public class NewFlightDTO : INewFlightDTO
    {
        public Guid Id { get; set; }      
        public FlightDirection Direction { get; set; }   
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
