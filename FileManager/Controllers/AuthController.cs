using Microsoft.AspNetCore.Mvc;
using FileManagerAPI.Models;
using FileManager;
using FileManagerLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FileManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("index")]
        public IActionResult Index([FromHeader]string username, [FromHeader]string password, PeopleContext peopleContext)
        {
            Person? person = peopleContext.People.Find(username, password);
            if (person is null)
                return Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = person.Email
            };

            return Json(response);
        }
        [HttpPost("register")]
        public IActionResult Register([FromHeader] string username, [FromHeader] string password, PeopleContext peopleContext)
        {
            Person? person = peopleContext.People.Find(username, password);

            if (person is not null)
                return BadRequest();

            if (peopleContext.People.Any(p => p.Email == username))
                return Unauthorized();

            person = new Person(username, password);
            peopleContext.People.Add(person);
            peopleContext.SaveChanges();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = person.Email
            };

            return Json(response);
        }
    }
}
