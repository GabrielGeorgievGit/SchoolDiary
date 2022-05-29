using Microsoft.AspNetCore.Mvc;
using WebAPI.Queries;

namespace WebAPI.Controllers
{
    [Route("api/school")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        public SchoolController()
        {
        }

        [HttpGet("")]
        public School getSchool()
        {
            Finder finder = new Finder();
            int? userId = Signer.user.id;
            if (userId == null) return null;
            School school = finder.findSchool(userId);
            Console.WriteLine("School name: " + school.Name);
            Console.WriteLine("School type: " + school.Type);
            return school;
        }

        [HttpPost("")]
        public IActionResult registerSchool(School school)
        {
            Console.WriteLine("School name: " + school.Name);
            Console.WriteLine("School type: " + school.Type);

            SchoolQuery schoolQuery = new SchoolQuery();
            School s = schoolQuery.registerSchool(school);

            if (s != null)
                return Created("~api/school", school);
            else return BadRequest("Couldn't add to database this school");
        }

        [HttpPut("")]
        public IActionResult editSchool(School school)
        {
            SchoolQuery schoolQuery = new SchoolQuery();
            schoolQuery.schoolEdit(school.Name, school.Type);

            return Created("~api/school", Finder.school);
        }
    }
}