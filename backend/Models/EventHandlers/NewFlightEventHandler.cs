using Common.Interfaces;
using System;

namespace Common.EventHandlers
{

    public delegate void NewFlightEventHandler(object sender, NewFlightEventArgs e);
    public class NewFlightEventArgs : EventArgs
    {
        public IFlightService FlightService { get; }
        public bool ToSave { get; set; }

        public NewFlightEventArgs(IFlightService flightService,bool toSave)
        {
            ToSave = toSave;
            FlightService = flightService;
        }
    }
}
