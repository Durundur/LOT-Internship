using AutoMapper;
using LOT_TASK.Dtos;
using LOT_TASK.Models;
using LOT_TASK.Reposiotories.Interfaces;
using LOT_TASK.Services.Interfaces;

namespace LOT_TASK.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public FlightService(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightDto>> GetAllFlights()
        {
            var flights = await _flightRepository.GetAllFlights();
            return _mapper.Map<IEnumerable<FlightDto>>(flights);
        }

        public async Task<FlightDto> GetFlightById(Guid id)
        {
            var flight = await _flightRepository.GetFlightById(id);
            return _mapper.Map<FlightDto>(flight);
        }

        public async Task<FlightDto> CreateFlight(FlightDto flight)
        {
            var flightModel = _mapper.Map<FlightModel>(flight);
            var created = await _flightRepository.CreateFlight(flightModel);
            return _mapper.Map<FlightDto>(created);
        }

        public async Task<FlightDto> UpdateFlight(Guid id, FlightDto flight)
        {
            var flightModel = _mapper.Map<FlightModel>(flight);
            var updatedFlight = await _flightRepository.UpdateFlight(id, flightModel);
            return _mapper.Map<FlightDto>(updatedFlight);
        }

        public async Task<bool> DeleteFlight(Guid id)
        {
            return await _flightRepository.DeleteFlight(id);
        }
    }
}
