using LOT_TASK.Dtos;

namespace LOT_TASK.Services.Interfaces
{
    public interface IFlightService
    {
        Task<FlightDto> CreateFlight(FlightDto flight);
        Task<bool> DeleteFlight(Guid id);
        Task<IEnumerable<FlightDto>> GetAllFlights();
        Task<FlightDto> GetFlightById(Guid id);
        Task<FlightDto> UpdateFlight(Guid id, FlightDto flight);
    }
}