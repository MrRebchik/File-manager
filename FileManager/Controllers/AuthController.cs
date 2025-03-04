﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FileManagerAPI.Models;
using FileManager;
using FileManagerLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost]
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
    }
}
