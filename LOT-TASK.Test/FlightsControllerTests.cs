using LOT_TASK.Controllers;
using LOT_TASK.Dtos;
using LOT_TASK.Services;
using LOT_TASK.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace LOT_TASK.Test
{
    public class FlightsControllerTests
    {

        private Mock<IFlightService> _mockFlightService;
        private FlightsController _flightController;
        public FlightsControllerTests()
        {
            _mockFlightService = new Mock<IFlightService>();
            _flightController = new FlightsController(_mockFlightService.Object);
        }

        [Fact]
        public async Task GetAllFlights_ReturnsOkResultWithFlights()
        {
            // Arrange
            var flights = new List<FlightDto> { new FlightDto { Id = Guid.NewGuid() }, new FlightDto { Id = Guid.NewGuid() } };
            _mockFlightService.Setup(service => service.GetAllFlights()).ReturnsAsync(flights);

            // Act
            var result = await _flightController.GetAllFlights();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFlights = Assert.IsAssignableFrom<IEnumerable<FlightDto>>(okResult.Value);
            Assert.Equal(flights.Count, returnedFlights.Count());
        }

        [Fact]
        public async Task GetFlightById_WithExistingId_ReturnsOkResultWithFlight()
        {
            // Arrange
            var existingId = Guid.NewGuid();
            var flightDto = new FlightDto { Id = existingId };
            _mockFlightService.Setup(service => service.GetFlightById(existingId)).ReturnsAsync(flightDto);

            // Act
            var result = await _flightController.GetFlightById(existingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFlight = Assert.IsType<FlightDto>(okResult.Value);
            Assert.Equal(existingId, returnedFlight.Id);
        }

        [Fact]
        public async Task GetFlightById_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _mockFlightService.Setup(service => service.GetFlightById(nonExistingId)).ReturnsAsync((FlightDto)null);

            // Act
            var result = await _flightController.GetFlightById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateFlight_WithValidFlight_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var validFlight = new FlightDto { FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };
            var createdFlight = new FlightDto { Id = Guid.NewGuid(), FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };
            _mockFlightService.Setup(service => service.CreateFlight(validFlight)).ReturnsAsync(createdFlight);

            // Act
            var result = await _flightController.CreateFlight(validFlight);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedFlight = Assert.IsType<FlightDto>(createdAtActionResult.Value);
            Assert.Equal(createdFlight.Id, returnedFlight.Id);
            Assert.Equal(createdFlight, returnedFlight);
        }

        [Fact]
        public async Task CreateFlight_WithInvalidFlight_ReturnsBadRequestResult()
        {
            // Arrange
            var invalidFlight = new FlightDto();
            _flightController.ModelState.AddModelError("FlightNumber", "Flight number is required.");

            // Act
            var result = await _flightController.CreateFlight(invalidFlight);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateFlight_WithValidFlight_ReturnsOkResultWithUpdatedFlight()
        {
            // Arrange
            var id = Guid.NewGuid();
            var validFlight = new FlightDto { Id = id, FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };
            _mockFlightService.Setup(service => service.UpdateFlight(id, validFlight)).ReturnsAsync(validFlight);

            // Act
            var result = await _flightController.UpdateFlight(id, validFlight);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedFlight = Assert.IsType<FlightDto>(okResult.Value);
            Assert.Equal(id, updatedFlight.Id);
            Assert.Equal(validFlight, updatedFlight);
        }


        [Fact]
        public async Task UpdateFlight_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var invalidId = Guid.NewGuid();
            var validFlight = new FlightDto { Id = id, FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };

            // Act
            var result = await _flightController.UpdateFlight(invalidId, validFlight);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateFlight_WithNonExistingFlight_ReturnsNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var nonExistingFlight = new FlightDto { Id = id, FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };
            _mockFlightService.Setup(service => service.UpdateFlight(id, nonExistingFlight)).ReturnsAsync((FlightDto)null);

            // Act
            var result = await _flightController.UpdateFlight(id, nonExistingFlight);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFlight_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingFlight = new FlightDto { Id = id, FlightNumber = "ABC123", DepartureDate = DateTime.Now, DepartureLocation = "BERLIN", ArrivalLocation = "WARSAW", AircraftType = "airbus" };
            _mockFlightService.Setup(service => service.GetFlightById(id)).ReturnsAsync(existingFlight);

            // Act
            var result = await _flightController.DeleteFlight(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFlight_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _mockFlightService.Setup(service => service.GetFlightById(nonExistingId)).ReturnsAsync((FlightDto)null);

            // Act
            var result = await _flightController.DeleteFlight(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

    }

}
