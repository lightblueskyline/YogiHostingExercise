using APIConsume.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7244/api/Reservation"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiResponse) ?? new List<Reservation>();
                }
            }
            return View(reservationList);
        }

        #region GetReservation
        [HttpGet]
        public ViewResult GetReservation() => View();

        [HttpPost]
        public async Task<IActionResult> GetReservation(int id)
        {
            Reservation reservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7244/api/Reservation/{id}"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        reservation = JsonConvert.DeserializeObject<Reservation>(apiResponse) ?? new Reservation();
                    }
                    else
                    {
                        ViewBag.StatusCode = response.StatusCode;
                    }
                    ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View(reservation);
        }
        #endregion

        #region AddReservation
        public ViewResult AddReservation() => View();

        [HttpPost]
        public async Task<IActionResult> AddReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", "Secret@123");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7244/api/Reservation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse) ?? new Reservation();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                }
            }
            return View(receivedReservation);
        }
        #endregion

        #region UpdateReservation
        [HttpGet]
        public async Task<IActionResult> UpdateReservation(int id)
        {
            Reservation reservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7244/api/Reservation/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<Reservation>(apiResponse) ?? new Reservation();
                }
            }
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(reservation.Id.ToString()), "Id");
                content.Add(new StringContent(reservation.Name), "Name");
                content.Add(new StringContent(reservation.StartLocation), "StartLocation");
                content.Add(new StringContent(reservation.EndLocation), "EndLocation");

                using (var response = await httpClient.PutAsync("https://localhost:7244/api/Reservation/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse) ?? new Reservation();
                }
            }
            return View(receivedReservation);
        }
        #endregion

        #region UpdateReservationPatch
        [HttpGet]
        public async Task<IActionResult> UpdateReservationPatch(int id)
        {
            Reservation reservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7244/api/Reservation/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<Reservation>(apiResponse) ?? new Reservation();
                }
            }
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReservationPatch(int id, Reservation reservation)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://localhost:7244/api/Reservation/" + id),
                    Method = new HttpMethod("Patch"),
                    Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"Name\", \"value\": \"" + reservation.Name + "\"},{ \"op\": \"replace\", \"path\": \"StartLocation\", \"value\": \"" + reservation.StartLocation + "\"}]", Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteReservation
        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int ReservationId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7244/api/Reservation/" + ReservationId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region AddFile
        [HttpGet]
        public ViewResult AddFile() => View();

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            string apiResponse = "";
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                using (var fileStream = file.OpenReadStream())
                {
                    form.Add(new StreamContent(fileStream), "file", file.FileName);
                    using (var response = await httpClient.PostAsync("https://localhost:7244/api/Reservation/UploadFile", form))
                    {
                        response.EnsureSuccessStatusCode();
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return View((object)apiResponse);
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
