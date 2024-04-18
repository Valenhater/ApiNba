using ApiNba.Models;
using ApiNba.Repositories;
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
        public async Task<ActionResult<Equipo>> FindDepartamento(int id)
        {
            return await this.repo.ObtenerEquipoPorId(id);
        }
    }
}
