using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidosController : ControllerBase
    {
        private RepositoryNba repo;
        public PartidosController(RepositoryNba repo)
        {
            this.repo = repo;
        }
        [HttpGet("GetPartidos")]
        public async Task<ActionResult<List<Partido>>> GetPartidosJugados()
        {
            return await this.repo.GetPartidosJugadosAsync();
        }
        [HttpGet("GetAllEquipos")]
        public async Task<ActionResult<List<Equipo>>> GetAllEquipos()
        {
            return await this.repo.GetAllEquiposAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> FindPartido(int id)
        {
            return await this.repo.ObtenerEquipoPorId(id);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertarEquipo(Equipo nuevoEquipo)
        {
            await this.repo.InsertarEquipoAsync(nuevoEquipo);
            return Ok();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarEquipo(int id, Equipo equipoModificado)
        {
            if (id != equipoModificado.IdEquipo)
            {
                return BadRequest();
            }

            try
            {
                await this.repo.ModificarEquipoAsync(equipoModificado);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEquipo(int id)
        {
            try
            {
                await repo.EliminarEquipoAsync(id);
                return NoContent(); // Retorna NoContent si la eliminación fue exitosa
            }
            catch (InvalidOperationException)
            {
                return NotFound(); // Retorna NotFound si el equipo no se encontró
            }
        }

    }
}
