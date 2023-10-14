using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingCards.Models.Domain;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;

namespace TradingCards.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;

        public AuthController(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration["ApiSettings:Secret"];                
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (userFromDb != null)
            {
                return BadRequest("Username already exists");
            }

            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        //create roles in database
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("collector"));
                    }
                    if (model.Role.ToLower() == "admin")
                    {
                        await _userManager.AddToRoleAsync(newUser, "admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, "collector");
                    }

                    // Create a default collection
                    await _db.Collections.AddAsync(new Collection() { Name = model.Name, UserId = newUser.Id });
                    await _db.SaveChangesAsync();

                    return Ok();
                }
            }
            catch (Exception)
            {

            }

            return BadRequest("Error while registering");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            //ApplicationUser userFromDb = _db.ApplicationUsers
            //        .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            //bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

            //if (isValid == false)
            //{
            //    //_response.Result = new LoginResponseDTO();
            //    //_response.StatusCode = HttpStatusCode.BadRequest;
            //    //_response.IsSuccess = false;
            //    //_response.ErrorMessages.Add("Username or password is incorrect");
            //    return BadRequest("Username or password is incorrect");
            //}

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) )
            {
                return Unauthorized();
            }



            //we have to generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("fullName", user.Name),
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Email = user.Email,
                Token = tokenHandler.WriteToken(token)
            };

            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                //_response.StatusCode = HttpStatusCode.BadRequest;
                //_response.IsSuccess = false;
                //_response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest("Username or password is incorrect");
            }

            //_response.StatusCode = HttpStatusCode.OK;
            //_response.IsSuccess = true;
            //_response.Result = loginResponse;
            return Ok(loginResponse);

        }
    }
}
