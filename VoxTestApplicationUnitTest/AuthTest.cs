using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using VoxTestApplication.Controllers;
using VoxTestApplication.Helpers;
using VoxTestApplication.Models.Auth;

namespace VoxTestApplicationUnitTest
{
    public class AuthTest
    {
        [Fact]
        public void Login_ReturnsRedirectToUser_WhenLoginSuccess()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ApiBaseUrl", "https://api-sport-events.php6-02.test.voxteneo.com/api/v1"}
            };

            Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var mockLoginHttpClient = new Mock<IHttpClientHelper<LoginRequest>>();
            var mockRegisterHttpClient = new Mock<IHttpClientHelper<RegisterRequest>>();

            var controller = new AuthController(configuration, mockLoginHttpClient.Object, mockRegisterHttpClient.Object);

            var credential = GetLoginUser();

            //Act
            var result = controller.Login(credential, string.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockLoginHttpClient.Verify();
            mockRegisterHttpClient.Verify();
        }

        private LoginRequest GetLoginUser()
        {
            return new LoginRequest()
            {
                Email = "khoirudi16@gmail.com",
                Password = "Ojolali123!"
            };
        }
    }
}