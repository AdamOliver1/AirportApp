using Common.Enums;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extentions
{
    public static class DirectionConverter
    {
       public static DirectionAllOptions Convert(this FlightDirection direction)
        {
            return direction == FlightDirection.Landing ?
                DirectionAllOptions.Landing :
                DirectionAllOptions.Takeoff;
        }
    }
}
