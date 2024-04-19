using ApiNba.Helpers;
using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiNba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperActionServicesOAuth helper;
        public AuthController(RepositoryUsuarios repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            //BUSCAMOS AL Usuario EN NUESTRO REPO 
            bool usuario = await this.repo.LogInUserAsync(model.UserName, model.Password);

            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA  
                //INCLUIRLAS DENTRO DEL TOKEN Y QUE ESTARAN  
                //COMPUESTAS POR EL SECRET KEY CIFRADO Y EL TIPO 
                //DE CIFRADO QUE DESEEMOS INCLUIR EN EL TOKEN 
                SigningCredentials credentials =
                    new SigningCredentials(
                        this.helper.GetKeyToken()
                        , SecurityAlgorithms.HmacSha256);
                //EL TOKEN SE GENERA CON UNA CLASE Y  
                //DEBEMOS INDICAR LOS ELEMENTOS QUE ALMACENARA  
                //DENTRO DE DICHO TOKEN, POR EJEMPLO, ISSUER, 
                //AUDIENCE O EL TIEMPO DE VALIDACION DEL TOKEN 
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                //POR ULTIMO, DEVOLVEMOS UNA RESPUESTA AFIRMATIVA 
                //CON UN OBJETO ANONIMO EN FORMATO JSON 
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler()
                        .WriteToken(token)
                    });
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            try
            {
                // Verificar si el nombre de usuario ya está en uso
                if (await this.repo.UsuarioExistsAsync(model.UserName))
                {
                    return BadRequest("El nombre de usuario ya está en uso.");
                }

                // Verificar si el correo electrónico ya está en uso
                if (await this.repo.EmailExistsAsync(model.Email))
                {
                    return BadRequest("El correo electrónico ya está en uso.");
                }
                // Registrar el nuevo usuario
                var newUser = await this.repo.RegisterUserAsync(model.UserName, model.Password, model.Email, model.NombreCompleto, model.Direccion);
                // Devolver el usuario registrado
                return Ok(new { Usuario = newUser });
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolver un error interno del servidor
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<Usuario>> VerPerfil(string username)
        {
            var usuario = await repo.GetUser(username);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }
        [HttpPut("{username}")]
        public async Task<IActionResult> EditarPerfil(string username, Usuario usuario)
        {
            if (username != usuario.Nombre)
            {
                return BadRequest();
            }

            var perfilExistente = await repo.GetUser(username);

            if (perfilExistente == null)
            {
                return NotFound();
            }

            // Aplicar las modificaciones al perfil existente
            perfilExistente.NombreCompleto = usuario.NombreCompleto;
            perfilExistente.Email = usuario.Email;
            perfilExistente.Direccion = usuario.Direccion;

            // Guardar los cambios en la base de datos
            try
            {
                await repo.SaveChangesAsync();
            }
            catch
            {
                // Manejar errores en caso de fallo al guardar los cambios
                return StatusCode(500, "Error al guardar los cambios en el perfil.");
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await repo.DeleteUserAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}

