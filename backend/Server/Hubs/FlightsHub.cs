using Common.Interfaces.Hubs;
using Common.Interfaces.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs.HubTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class FlightsHub :Hub<IAirportClient>, IFlightHub
    {
        readonly IHubContext<FlightsHub, IAirportClient> _context;

        public FlightsHub(IHubContext<FlightsHub, IAirportClient> context)
        {
            _context = context; 
        }
        
        public Task NotifyFlightMovemont(IFlightInStationDTO flightInStationDTO)
        {
            
            return _context.Clients.All.RecieveFlightMovement(flightInStationDTO);
        }
        public Task NotifiyNewFlight(INewFlightDTO newFlightDTO)
        {           
            return _context.Clients.All.RecieveNewFlight(newFlightDTO);
        }

        public Task NotifyAirportStations(List<IStationDTO> listOfStations)
        {
            return _context.Clients.All.RecieveAirportStations(listOfStations);
        }

       
    }
}
