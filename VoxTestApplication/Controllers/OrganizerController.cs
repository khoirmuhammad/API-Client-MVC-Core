using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Text;
using VoxTestApplication.Helpers;
using VoxTestApplication.Models.Auth;
using VoxTestApplication.Models;
using VoxTestApplication.Models.Organizer;
using VoxTestApplication.Models.User;
using AutoMapper;

namespace VoxTestApplication.Controllers
{
    [Authorize]
    public class OrganizerController : Controller
    {
        private readonly string? _apiBaseUrl;
        private readonly IHttpClientHelper<OrganizerResponse> _clientOrganizer;
        private readonly IHttpClientHelper<OrganizerCreateRequest> _clientCreate;
        private readonly IMapper _mapper;
        private int _perPage = 10;

        public OrganizerController(IConfiguration configuration,
            IHttpClientHelper<OrganizerResponse> clientOrganizer,
            IHttpClientHelper<OrganizerCreateRequest> clientCreate,
            IMapper mapper)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _clientOrganizer = clientOrganizer;
            _clientCreate = clientCreate;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;

            string url = $"{_apiBaseUrl}/organizers?page={page}&perPage={_perPage}";

            HttpResponseMessage message = await _clientOrganizer.GetAsync(accessToken, url);

            OrganizerResponse? response = await message.Content.ReadFromJsonAsync<OrganizerResponse>();

            List<Organizer> data = response != null ? response.Data : new List<Organizer>();

            ViewData["Page"] = page;
            ViewData["TotalPages"] = response == null ? 0 : response.Meta.Pagination.Total_Pages;
            ViewData["CurrentPage"] = response == null ? 0 : response.Meta.Pagination.Current_Page;

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Organizer model)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers";

            OrganizerCreateRequest body = _mapper.Map<OrganizerCreateRequest>(model);

            HttpResponseMessage message = await _clientCreate.PostAsync(accessToken, url, body);

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = await message.Content.ReadFromJsonAsync<Organizer>();

                ViewBag.SuccessMessage = $"Organizer {response.Id} - {response.OrganizerName} has been created";
                ModelState.Clear();
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<ApiResponse<Organizer>>();

                StringBuilder sb = new StringBuilder();

                if (response != null)
                {
                    sb.AppendLine(response.Message);
                    sb.AppendLine(string.Join(",", response.Errors.OrganizerName == null ? new List<string>() : response.Errors.OrganizerName));
                    sb.AppendLine(string.Join(",", response.Errors.ImageLocation == null ? new List<string>() : response.Errors.ImageLocation));
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View();
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers/{id}";

            HttpResponseMessage message = await _clientCreate.GetAsync(accessToken, url);

            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.ErrorMessage = "Organizer Not Found";
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<Organizer>();
                return View(response);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers/{id}";

            HttpResponseMessage message = await _clientCreate.GetAsync(accessToken, url);

            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.ErrorMessage = "Organizer Not Found";
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<Organizer>();
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Organizer model)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers/{model.Id}";

            OrganizerCreateRequest body = _mapper.Map<OrganizerCreateRequest>(model);

            HttpResponseMessage message = await _clientCreate.PutAsync(accessToken, url, body);

            if (message.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                ViewBag.SuccessMessage = $"Organizer {model.Id} has been updated";
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<ApiResponse<Organizer>>();

                StringBuilder sb = new StringBuilder();

                if (response != null)
                {
                    sb.AppendLine(response.Message);
                    sb.AppendLine(string.Join(",", response.Errors.OrganizerName == null ? new List<string>() : response.Errors.OrganizerName));
                    sb.AppendLine(string.Join(",", response.Errors.ImageLocation == null ? new List<string>() : response.Errors.ImageLocation));
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers/{id}";

            HttpResponseMessage message = await _clientCreate.GetAsync(accessToken, url);

            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.ErrorMessage = "Organizer Not Found";
                return View();
            }
            else
            {
                var response = await message.Content.ReadFromJsonAsync<Organizer>();
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Organizer model)
        {
            string? accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
            string url = $"{_apiBaseUrl}/organizers/{model.Id}";

            HttpResponseMessage message = await _clientCreate.DeleteAsync(accessToken, url, null);

            if (message.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                TempData["SuccessMessage"] = $"Organizer {model.Id} - {model.OrganizerName} has been deleted";
                return RedirectToAction("Index", "Organizer");
            }
            else
            {

                ViewBag.ErrorMessage = $"Organizer {model.Id} - {model.OrganizerName} can't be deleted";
                return View();
            }
        }

    }
}
