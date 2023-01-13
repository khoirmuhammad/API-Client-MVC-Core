using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using VoxTestApplication.Helpers;
using VoxTestApplication.Models;
using VoxTestApplication.Models.Auth;
using VoxTestApplication.Models.User;

namespace VoxTestApplication.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly string? _apiBaseUrl;
        private readonly IHttpClientHelper<UserResponse> _clientUser;
        private readonly IHttpClientHelper<UserUpdateRequest> _clientUserUpdate;
        private readonly IHttpClientHelper<UserChangePasswordRequest> _clientChangePassword;
        public UserController(IConfiguration configuration, 
            IHttpClientHelper<UserResponse> clientUser, 
            IHttpClientHelper<RegisterRequest> clientUserCreate,
            IHttpClientHelper<UserUpdateRequest> clientUserUpdate,
            IHttpClientHelper<UserChangePasswordRequest> clientChangePassword)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _clientUser = clientUser;
            _clientUserUpdate = clientUserUpdate;
            _clientChangePassword = clientChangePassword;
        }
        public async Task<IActionResult> Index()
        {
            //throw new Exception("Dummy Error");
            string? id = HttpContext.User.FindFirst("Id")?.Value;
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;

            string url = $"{_apiBaseUrl}/users/{id}";     

            HttpResponseMessage message = await _clientUser.GetAsync(accessToken, url);

            UserResponse? response = await message.Content.ReadFromJsonAsync<UserResponse>();      

            return View(response);
        }

        public async Task<IActionResult> Edit(int id)
        {
            string url = $"{_apiBaseUrl}/users/{id}";

            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;

            HttpResponseMessage message = await _clientUser.GetAsync(accessToken, url);

            UserResponse? response = await message.Content.ReadFromJsonAsync<UserResponse>();

            UserUpdateRequest data = new UserUpdateRequest()
            {
                Id = response.Id,
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateRequest model)
        {
            string url = $"{_apiBaseUrl}/users/{model.Id}";

            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;

            HttpResponseMessage message = await _clientUserUpdate.PutAsync(accessToken, url, model);
        

            if (message.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<ApiResponse<UserUpdateErrorResponse>>();

                StringBuilder sb = new StringBuilder();

                if (response != null)
                {
                    sb.AppendLine(response.Message);
                    sb.AppendLine(string.Join(",", response.Errors.FirstName == null ? new List<string>() : response.Errors.FirstName));
                    sb.AppendLine(string.Join(",", response.Errors.LastName == null ? new List<string>() : response.Errors.LastName));
                    sb.AppendLine(string.Join(",", response.Errors.Email == null ? new List<string>() : response.Errors.Email));
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View();
            }
        }

        public IActionResult ChangePassword(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserChangePasswordRequest model)
        {
            string url = $"{_apiBaseUrl}/users/{model.Id}/password";

            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;

            HttpResponseMessage message = await _clientChangePassword.PutAsync(accessToken, url, model);

            if (message.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<ApiResponse<UserChangePasswordErrorResponse>>();

                StringBuilder sb = new StringBuilder();

                if (response != null)
                {
                    sb.AppendLine(response.Message);
                    sb.AppendLine(string.Join(",", response.Errors.OldPassword == null ? new List<string>() : response.Errors.OldPassword));
                    sb.AppendLine(string.Join(",", response.Errors.NewPassword == null ? new List<string>() : response.Errors.NewPassword));
                    sb.AppendLine(string.Join(",", response.Errors.RepeatPassword == null ? new List<string>() : response.Errors.RepeatPassword));
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View();
            }
        }


    }
}
