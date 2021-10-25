using Common.API.Models.DTO;
using Common.Interfaces;
using Common.Interfaces.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirPortController : ControllerBase
    {
        private readonly ILogger<AirPortController> _logger;
        IDataService _dbService;

        public AirPortController(ILogger<AirPortController> logger, IDataService dbService)
        {
            _dbService = dbService;
            _logger = logger;
        }      

        [HttpPost("flight")]
        public async Task<IActionResult> AddNewFlight(Flight flight)
        {
            await _dbService.AddNewFlight(flight);
            return Ok();
        }

        [HttpGet("stations")]
        public IEnumerable<IStationDTO> GetStations()
        {
            return _dbService.GetStationsDTO();
        }

        [HttpGet("unfinishedflights")]
      
        public IEnumerable<IStationStateDTO> GetUnfinishedflights()
        {          
            return _dbService.GetUnfinishedFlights();
        }
    }
}
