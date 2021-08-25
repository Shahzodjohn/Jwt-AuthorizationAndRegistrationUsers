using Jwt.DTOs;
using Jwt.Models;
using Jwt.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                FirstName = registerDTO.Name,
                Email = registerDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
            };

            return Created("Success", _repository.Create(user));
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = _repository.GetByEmail(loginDTO.Email);
            if (user == null)
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }
        [HttpGet("User")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Varify(jwt);

                var UserId = int.Parse(token.Issuer);
                var user = _repository.GetById(UserId);
                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "success"
            });
        }

    }
}
