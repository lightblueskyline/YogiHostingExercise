using APIControllers.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace APIControllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private IRepository repository;
        private IWebHostEnvironment webHostEnvironment;

        public ReservationController(IRepository repo, IWebHostEnvironment environment)
        {
            repository = repo;
            webHostEnvironment = environment;
        }

        [HttpGet]
        // [Produces("application/xml")] // Web API to return the data in XML
        public IEnumerable<Reservation> Get() => repository.Reservations;

        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Value must be passed in the request body.");
            }

            return Ok(repository[id]);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Reservation res)
        {
            if (!Authenticate())
            {
                return Unauthorized();
            }
            return Ok(repository.AddReservation(new Reservation
            {
                Name = res.Name,
                StartLocation = res.StartLocation,
                EndLocation = res.EndLocation
            }));
        }

        [HttpPut]
        public Reservation Put([FromForm] Reservation res) => repository.UpdateReservation(res);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<Reservation> patch)
        {
            var res = (Reservation)(Get(id).Result as OkObjectResult)?.Value!;
            if (res != null)
            {
                patch.ApplyTo(res);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteReservation(id);

        [HttpPost("UploadFile")]
        public async Task<string> UploadFile([FromForm] IFormFile file)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Images/" + file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "https://localhost:7244/Images/" + file.FileName;
        }

        private bool Authenticate()
        {
            var allowedKeys = new[] { "Secret@123", "Secret#123", "SecretABC" };
            StringValues key = Request.Headers["Key"];
            int count = (from t in allowedKeys where t == key select t).Count();
            return count == 0 ? false : true;
        }
    }
}
