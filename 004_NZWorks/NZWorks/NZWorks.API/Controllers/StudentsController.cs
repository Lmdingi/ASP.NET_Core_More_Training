using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            string[] students = ["Jhon", "Jane", "Mark", "Emily", "David"];
            return Ok(students);
        }
    }
}
