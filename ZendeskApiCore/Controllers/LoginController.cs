using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ESCORIALContext context, IConfiguration configuration, IMapper mapper, ILogger<LoginController> logger) : ControllerBase
    {
        // POST: api/Login
        /// <summary>
        /// Metodo de inicio de sesión y obtención de JWT para autenticación en API Zendesk Reclamos Web.
        /// </summary>
        /// <remarks>
        /// No requiere autenticación.
        /// Se debe indicar nombre de usuario y contraseña proporcionados por el sector de sistemas de Escorial. Se devolverá un JWT a utilizar posteriormente para autenticarse en los métodos de la API.
        /// </remarks>
        /// <param name="usuarioLogin">DTO con usuario y contraseña para inicio de sesión.</param>
        /// <returns>Token para autenticación en API, fecha de expiración en formato UTC y datos del usuario registrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se encontró el usuario. En caso de error o solicitud de registro, contactarse con sector de sistemas de Escorial.</response>
        /// <response code="400">BadRequest. Formato de objeto incorrecto o Id inexistente.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto usuarioLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (usuarioLogin is null)
                    return BadRequest("No se proporcionó el UserLoginDto.");
                if (string.IsNullOrEmpty(usuarioLogin.User) || string.IsNullOrEmpty(usuarioLogin.Password))
                    return BadRequest("No se indicó usuario o contraseña.");
                var userInfo = await AutenticarUsuarioAsync(usuarioLogin.User, usuarioLogin.Password);
                if (userInfo != null)
                {
                    var userInfoDto = mapper.Map<UserInfoDto>(userInfo);
                    var (token, expiration) = GenerarTokenJWT(userInfo);
                    return Ok(new { token, expirationUtc = expiration, userInfo = userInfoDto });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método Login para el usuario {Usuario}", usuarioLogin?.User);
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        private async Task<Login?> AutenticarUsuarioAsync(string usuario, string password)
        {
            var user = await context.Login.FirstOrDefaultAsync(x => x.User.Equals(usuario) && x.Password.Equals(password));
            if (user is null)
                return null;
            return mapper.Map<Login>(user);
        }

        private (string token, DateTime expiration) GenerarTokenJWT(Login usuarioInfo)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"]!)
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            var expirationUtc = DateTime.UtcNow.AddHours(24);
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()!),
                new Claim("nombre", usuarioInfo.Name),
                new Claim("apellidos", usuarioInfo.LastName),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Mail),
                new Claim(ClaimTypes.Role, usuarioInfo.Rol)
            };
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    expires: expirationUtc
                );
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(_Token);
            return (tokenString, expirationUtc);
        }
    }
}