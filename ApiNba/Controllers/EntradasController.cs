using ApiNba.Models;
using ApiNba.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("GetEntrada/{id}")]
        public async Task<ActionResult<ModelVistaProximosPartidos>> VistaEntrada(int id)
        {
            return await this.repo.FindPartidoAsync(id);
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
        [HttpGet("GetEntradasReservadas/{idusuario}")]
        public async Task<ActionResult<List<VistaEntradasReservadas>>> EntradasReservadas(int idusuario)
        {
            var reservas = await this.repo.GetReservasEntradasAsync(idusuario);
            return Ok(reservas);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartido(int id)
        {
            
                await this.repo.EliminarReservaEntradaAsync(id);
                return Ok();
            
        }
        [HttpGet("partidosfavoritos")]
        public async Task<ActionResult<List<ModelVistaProximosPartidos>>> GetPartidosFavoritos()
        {
            var partidosFavoritos = await this.repo.GetPartidosFavoritosAsync();
            if (partidosFavoritos == null)
            {
                return NotFound("No hay partidos almacenados en el carrito.");
            }
            return Ok(partidosFavoritos);
        }
    }
}
