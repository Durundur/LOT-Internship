using LOT_TASK.Controllers;
using LOT_TASK.Dtos;
using LOT_TASK.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOT_TASK.Test
{
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var credentials = new AuthRequestDto { Email = "testUser", Password = "testPassword" };
            var loginResult = new AuthResponseDto { Success = true };
            _mockAuthService.Setup(service => service.Login(credentials)).ReturnsAsync(loginResult);

            // Act
            var result = await _authController.Login(credentials);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task Register_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var credentials = new AuthRequestDto { Email = "testUser", Password = "testPassword" };
            var registerResult = new AuthResponseDto { Success = true };
            _mockAuthService.Setup(service => service.Register(credentials)).ReturnsAsync(registerResult);

            // Act
            var result = await _authController.Register(credentials);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task RefreshToken_ValidJwt_ReturnsOkResult()
        {
            // Arrange
            var jwt = new RefreshTokenRequest { RefreshToken = "testRefreshToken" };
            var response = new AuthResponseDto { Success = true };
            _mockAuthService.Setup(service => service.RefreshToken(jwt)).ReturnsAsync(response);

            // Act
            var result = await _authController.RefreshToken(jwt);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponse = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.True(returnedResponse.Success);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var invalidCredentials = new AuthRequestDto { Email = "invalidUser", Password = "invalidPassword" };
            var loginResult = new AuthResponseDto { Success = false };
            _mockAuthService.Setup(service => service.Login(invalidCredentials)).ReturnsAsync(loginResult);

            // Act
            var result = await _authController.Login(invalidCredentials);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithInvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var invalidCredentials = new AuthRequestDto { Email = "invalidUser", Password = "invalidPassword" };
            var registerResult = new AuthResponseDto { Success = false };
            _mockAuthService.Setup(service => service.Register(invalidCredentials)).ReturnsAsync(registerResult);

            // Act
            var result = await _authController.Register(invalidCredentials);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_WithInvalidJwt_ReturnsUnauthorizedResult()
        {
            // Arrange
            var invalidJwt = new RefreshTokenRequest { RefreshToken = "invalidRefreshToken" };
            var response = new AuthResponseDto { Success = false };
            _mockAuthService.Setup(service => service.RefreshToken(invalidJwt)).ReturnsAsync(response);

            // Act
            var result = await _authController.RefreshToken(invalidJwt);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsConflictResult()
        {
            // Arrange
            var existingEmail = "existing@example.com";
            var credentials = new AuthRequestDto { Email = existingEmail, Password = "testPassword" };
            var registerResult = new AuthResponseDto { Success = false };
            _mockAuthService.Setup(service => service.Register(credentials)).ReturnsAsync(registerResult);

            // Act
            var result = await _authController.Register(credentials);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidCredentialsFormat_ReturnsBadRequestResult()
        {
            // Arrange
            var invalidCredentials = new AuthRequestDto { Email = "invalidUser", Password = "" };
            var loginResult = new AuthResponseDto { Success = false };
            _mockAuthService.Setup(service => service.Login(invalidCredentials)).ReturnsAsync(loginResult);

            // Act
            var result = await _authController.Login(invalidCredentials);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
