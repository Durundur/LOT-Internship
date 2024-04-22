using LOT_TASK.Models;

namespace LOT_TASK.Reposiotories.Interfaces
{
    public interface IFlightRepository
    {
        Task<FlightModel> CreateFlight(FlightModel flight);
        Task<bool> DeleteFlight(Guid id);
        Task<IEnumerable<FlightModel>> GetAllFlights();
        Task<FlightModel> GetFlightById(Guid id);
        Task<FlightModel> UpdateFlight(Guid id, FlightModel flight);
    }
}