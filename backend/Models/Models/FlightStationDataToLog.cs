using Models.Enums;
using System;

namespace Models
{
    public class FlightStationDataToLog
    {       
        public int Id { get; set; }
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }        
        public int? ToStationId{ get; set; }
        public Station ToStation { get; set; }
        public int? FromStationId { get; set; }
        public Station FromStation { get; set; }
        public FlightDirection Direction { get; set; }
        public DateTime EntranceStaionDate { get; set; }      
    }
}
