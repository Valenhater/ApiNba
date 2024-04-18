using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasController : ControllerBase
    {
        private RepositoryEntradas repo;
        public EntradasController(RepositoryEntradas repo)
        {
            this.repo = repo;
        }
        [HttpGet("GetEntradas")]
        public async Task<ActionResult<List<ModelVistaProximosPartidos>>> VistaReservaEntradas()
        {
            return await this.repo.GetProximosPartidosAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ModelVistaProximosPartidos>>> PartidosFavoritos(List<int> id)
        {
            return await this.repo.GetFavoritosAsync(id);
        }
        [HttpPost]
        public async Task<ActionResult> ReservarEntradas(ReservaEntrada reserva)
        {
            await this.repo.ReservarEntradaAsync(reserva.UsuarioId,reserva.PartidoId,reserva.Asiento);
            return Ok();
        }
        [HttpGet("GetEntradasReservadas")]
        public async Task<ActionResult<List<ReservaEntrada>>> EntradasReservadas()
        {
            return await this.repo.GetReservasEntradasAsync();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartido(int id)
        {
            if (await this.repo.FindPartidoAsync(id) == null)
            {
                return NotFound();
            }
            else
            {
                await this.repo.EliminarReservaEntradaAsync(id);
                return Ok();
            }
        }
    }
}
