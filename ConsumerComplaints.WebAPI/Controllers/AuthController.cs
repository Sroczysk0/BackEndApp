using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ConsumerComplaints.Core.DTOs;
using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.WebAPI.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConsumerComplaints.WebAPI.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za rejestrację i logowanie użytkowników oraz generowanie tokenów JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Rejestracja nowego użytkownika i przypisywanie go do roli "User".
        /// </summary>
        /// <param name="dto">Dane użytkownika (login, email, hasło, imię, nazwisko, itp.).</param>
        /// <returns>Informacja o sukcesie lub lista błędów walidacyjnych.</returns>
        /// <response code="200">Użytkownik zarejestrowany poprawnie</response>
        /// <response code="400">Błędy walidacyjne lub rejestracyjne</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new UserEntity
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Details = new UserDetails
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Country = dto.Country,
                    DateOfBirth = dto.DateOfBirth,
                    PhoneNumber = dto.PhoneNumber,
                    CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("✅ Użytkownik zarejestrowany z pełnymi danymi i przypisany do roli 'User'");
        }

        /// <summary>
        /// Loguje użytkownika i generuje token JWT z przypisanymi rolami.
        /// </summary>
        /// <param name="dto">Login (nazwa użytkownika lub e-mail) oraz hasło.</param>
        /// <returns>Token JWT jeśli logowanie się powiodło.</returns>
        /// <response code="200">Zwraca token JWT</response>
        /// <response code="401">Niepoprawne dane logowania</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(ConsumerComplaints.WebAPI.Dto.LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Login)
                       ?? await _userManager.FindByEmailAsync(dto.Login);

            if (user == null) return Unauthorized("Użytkownik nie istnieje");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Błędne dane logowania");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
