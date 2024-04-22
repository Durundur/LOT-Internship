using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LOT_TASK.Dtos;
using LOT_TASK.Services;
using LOT_TASK.Services.Interfaces;

namespace LOT_TASK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlights();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound(new ActionResultDto { Message = "Flight not found.", Success = false });
            }
            return Ok(flight);
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateFlight(FlightDto flight)
        {
            var createdFlight = await _flightService.CreateFlight(flight);
            if (createdFlight == null)
            {
                return BadRequest(new ActionResultDto { Message = "Failed to create flight.", Success = false });
            }
            return CreatedAtAction(nameof(CreateFlight), new { id = createdFlight.Id }, createdFlight);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> UpdateFlight(Guid id, FlightDto flight)
        {
            if (flight.Id == null || id != flight.Id)
            {
                return BadRequest(new ActionResultDto { Message = "Invalid resource ID.", Success = false });
            }
            var updatedFlight = await _flightService.UpdateFlight(id, flight);
            if (updatedFlight == null)
            {
                return NotFound(new ActionResultDto { Message = "Flight not found.", Success = false });
            }

            return Ok(updatedFlight);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFlight(Guid id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound(new ActionResultDto { Message = "Flight not found.", Success = false});
            }
            await _flightService.DeleteFlight(id);
            return Ok(new ActionResultDto{Message = "Successfully deleted.", Success = true});
        }
    }
}
