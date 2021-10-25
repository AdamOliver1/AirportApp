using Common.Models;
using System;
using System.Collections.Generic;

namespace Models
{
    public class Station 
    {
        public int Id { get; set; }      
        public string Name { get; set; }
        public TimeSpan WaitingTime { get; set; }       
        public StationState StationState { get; set; }
        public virtual ICollection<StationsRelation> StationsRelations { get; set; }      
    }
}

