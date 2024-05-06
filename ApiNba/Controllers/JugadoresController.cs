using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {
        private RepositoryJugadores repo;
        private RepositoryNba repoEq;
        public JugadoresController(RepositoryJugadores repo, RepositoryNba repoEq)
        {
            this.repo = repo;
            this.repoEq = repoEq;
        }
        [HttpGet]
        public async Task<ActionResult<List<Jugador>>> GetAllJugadores()
        {
            return await this.repo.GetAllJugadoresAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Jugador>> GetJugador(int id)
        {
            return await this.repo.ObtenerJugadorPorId(id);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Jugador>> InsertarJugador(Jugador nuevoJugador)
        {
            await this.repo.InsertarJugadorAsync(nuevoJugador);
            return CreatedAtAction(nameof(GetJugador), new { id = nuevoJugador.IdJugador }, nuevoJugador);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarJugador(int id, Jugador jugadorModificado)
        {
            if (id != jugadorModificado.IdJugador)
            {
                return BadRequest();
            }

            try
            {
                await this.repo.ModificarJugadorAsync(jugadorModificado);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarJugador(int id)
        {
            try
            {
                await repo.DeleteJugadorAsync(id);
                return NoContent(); // Retorna NoContent si la eliminación fue exitosa
            }
            catch (InvalidOperationException)
            {
                return NotFound(); // Retorna NotFound si el jugador no se encontró
            }
        }
    }
}
