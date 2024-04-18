using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ApiNba.Data;
using ApiNba.Models;
using System.Data;
using System.Diagnostics.Metrics;

namespace ApiNba.Repositories
{
    #region procedures
//    ALTER PROCEDURE SP_ALL_PARTIDOS_JUGADOS
//    AS
//BEGIN
//    -- Seleccionar los detalles de los equipos involucrados en el partido
//    SELECT
//        P.EquipoLocalID,
//        EL.Nombre AS NombreEquipoLocal,
//        P.PuntosLocal,
//        P.EquipoVisitanteID,
//        EV.Nombre AS NombreEquipoVisitante,
//        P.PuntosVisitante
//    FROM
//        Partidos AS P
//    INNER JOIN
//        Equipos AS EL ON P.EquipoLocalID = EL.EquipoID
//    INNER JOIN
//        Equipos AS EV ON P.EquipoVisitanteID = EV.EquipoID;
//    END

    #endregion
    public class RepositoryNba
    {
        private NbaContext context;
        public RepositoryNba(NbaContext context)
        {
            this.context = context;
        }
        public async Task<List<Partido>> GetPartidosJugadosAsync()
        {
            var consulta = await context.Partidos
                                               .Include(p => p.EquipoLocal)
                                               .Include(p => p.EquipoVisitante)
                                               .ToListAsync();

            return consulta;
        }

        public async Task<List<Equipo>> GetAllEquiposAsync()
        {   
            var consulta = await this.context.Equipos.ToListAsync();
            return consulta;
        }
        public async Task<Equipo> ObtenerEquipoPorId(int id)
        {
            var consulta = this.context.Equipos.FirstOrDefault(e => e.IdEquipo == id);
            return consulta;
        }
    }
}
