using Models.Enums;

namespace Models
{
    public class StationsRelation
    {              
        public int ToStationId { get; set; }
        public virtual Station ToStation { get; set; }
      
        public int? FromStationId { get; set; }
        public virtual Station FromStation { get; set; }

        public FlightDirection Direction { get; set; }

    }
}
