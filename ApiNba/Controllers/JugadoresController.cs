using ApiNba.Models;
using ApiNba.Repositories;
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
    }
}
