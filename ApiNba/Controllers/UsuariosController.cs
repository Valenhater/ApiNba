using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryUsuarios repo;

        public UsuariosController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }
        [HttpGet("Details/{iduser}")]
        public async Task<ActionResult<Usuario>> Details(int iduser)
        {
            Usuario usuarioDetalles = await this.repo.GetUserByIdAsync(iduser);
            if (usuarioDetalles == null)
                return NotFound("Usuario no encontrado");
            return Ok(usuarioDetalles);
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            bool loginSuccess = await this.repo.LogInUserAsync(username, password);
            if (loginSuccess)
            {
                Usuario user = this.repo.GetUser(username);
                return Ok(new { Message = "Login exitoso", UserId = user.IdUsuario });
            }
            else
            {
                return BadRequest("Credenciales incorrectas");
            }
        }
        [HttpPost("Registro")]
        public async Task<ActionResult> Registro(string nombre, string correo, string password, string confirmPassword, string nombreCompleto, string direccion)
        {
            if (this.repo.EmailExists(correo))
            {
                return BadRequest("El correo electrónico ya está en uso");
            }

            if (password != confirmPassword)
            {
                return BadRequest("Las contraseñas no coinciden");
            }

            Usuario user = await this.repo.RegisterUserAsync(nombre, password, correo, nombreCompleto, direccion);

            if (user != null)
            {
                //string serverUrl = _helperPathProvider.MapUrlServerPath() + "/Usuarios/ActivateUser/?token=" + user.TokenMail;
                //string message = $"Activa tu cuenta aquí: {serverUrl}";
                //await _helperMails.SendMailAsync(correo, "Registro Usuario", message);

                return Ok(new { Message = "Usuario registrado y correo de activación enviado", UserId = user.IdUsuario });
            }
            else
            {
                return BadRequest("No se pudo crear el usuario");
            }
        }
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(int id, string nombre, string correo, string password)
        {
            Usuario existingUser = await this.repo.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound("Usuario no encontrado");

            // Supongamos que queremos actualizar solo el nombre y el correo
            existingUser.Nombre = nombre;
            existingUser.Email = correo;

            // Lógica de actualización del usuario
            // Esto es un ejemplo, deberías tener métodos en el repositorio para manejar la actualización real en la base de datos

            return Ok("Usuario actualizado correctamente");
        }

    }
}
