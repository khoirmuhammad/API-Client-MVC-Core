using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using VoxTestApplication.Models.Auth;
using System.Security.Claims;
using System.Net.Http.Headers;
using VoxTestApplication.Models.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Json;
using VoxTestApplication.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using System.Text;
using VoxTestApplication.Models;

namespace VoxTestApplication.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly string? _apiBaseUrl;
        private readonly IHttpClientHelper<LoginRequest> _clientLogin;
        private readonly IHttpClientHelper<RegisterRequest> _clientRegister;
        public AuthController(IConfiguration configuration, 
            IHttpClientHelper<LoginRequest> clientLogin,
            IHttpClientHelper<RegisterRequest> clientRegister)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _clientLogin = clientLogin;
            _clientRegister = clientRegister;
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            bool isAuthenticate = HttpContext.User.Identity.IsAuthenticated;

            if (isAuthenticate)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model, string returnUrl = null)
        {
            string url = $"{_apiBaseUrl}/users/login";
            string? accessToken = null;//HttpContext.User.FindFirst("AccessToken")?.Value;

            HttpResponseMessage message = await _clientLogin.PostAsync(accessToken, url, model);

            LoginResponse? response = await message.Content.ReadFromJsonAsync<LoginResponse>();

            if (string.IsNullOrEmpty(response?.Token))
            {
                ViewBag.ErrorMessage = "Please check your credential";
                return View("Index");
            }
                

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim("AccessToken", response.Token),
                new Claim("Id", Convert.ToString(response.Id))
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            string url = $"{_apiBaseUrl}/users";

            HttpResponseMessage message = await _clientRegister.PostAsync(null, url, model);

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.SuccessMessage = "User has been created. Please Login";
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<ApiResponse<RegisterErrorResponse>>();

                StringBuilder sb = new StringBuilder();

                if (response != null)
                {
                    sb.AppendLine(response.Message);
                    sb.AppendLine(string.Join(",", response.Errors.FirstName == null ? new List<string>() : response.Errors.FirstName));
                    sb.AppendLine(string.Join(",", response.Errors.LastName == null ? new List<string>() : response.Errors.LastName));
                    sb.AppendLine(string.Join(",", response.Errors.Email == null ? new List<string>() : response.Errors.Email));
                    sb.AppendLine(string.Join(",", response.Errors.Password == null ? new List<string>() : response.Errors.Password));
                    sb.AppendLine(string.Join(",", response.Errors.RepeatPassword == null ? new List<string>() : response.Errors.RepeatPassword));
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }


    }
}
