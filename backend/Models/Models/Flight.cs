using Common.Models;
using Models.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Flight 
    {
        public Guid Id  { get; set; }
        public string TakeoffDestination { get; set; }
        public string LandingDestination { get; set; }
        public FlightDirection Direction { get; set; }
        public DateTime Date { get; set; }
        public StationState StationState { get; set; }

        [NotMapped]
        public string Name { get => $"{TakeoffDestination} - {LandingDestination}"; }
       
    }
}
