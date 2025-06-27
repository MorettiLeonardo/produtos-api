using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using produtos_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Transactions;

namespace produtos_api.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(SignInManager<IdentityUser> singInManager,
                                UserManager<IdentityUser> userManager, 
                                IOptions<JwtSettings> jwtSettings)
        {
            _singInManager = singInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel regiterUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new IdentityUser
            {
                UserName = regiterUser.Email,
                Email = regiterUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, regiterUser.Password);

            if (result.Succeeded)
            {
                await _singInManager.SignInAsync(user, false);
                return Ok(GerarJwt());
            }

            return Problem("Falha ao registrar usuário");
        } 

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = _singInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            
            if (result == null)
            {
                return Problem("Erro ao fazer login");
            }
            
            return Ok(GerarJwt());

        }

        private string GerarJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
