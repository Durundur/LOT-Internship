using LOT_TASK.Models;
using LOT_TASK.Reposiotories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOT_TASK.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightModel>> GetAllFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<FlightModel> GetFlightById(Guid id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task<FlightModel> CreateFlight(FlightModel flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task<FlightModel> UpdateFlight(Guid id, FlightModel flight)
        {
            var existingFlight = await _context.Flights.FindAsync(id);
            if (existingFlight == null)
            {
                return null;
            }
            existingFlight.FlightNumber = flight.FlightNumber;
            existingFlight.DepartureDate = flight.DepartureDate;
            existingFlight.DepartureLocation = flight.DepartureLocation;
            existingFlight.ArrivalLocation = flight.ArrivalLocation;
            existingFlight.AircraftType = flight.AircraftType;
            await _context.SaveChangesAsync();
            return existingFlight;
        }

        public async Task<bool> DeleteFlight(Guid id)
        {
            var flightToDelete = await _context.Flights.FindAsync(id);
            if (flightToDelete != null)
            {
                _context.Flights.Remove(flightToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
