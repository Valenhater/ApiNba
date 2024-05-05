using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ApiNba.Data;
using ApiNba.Models;
using System.Data;

namespace ApiNba.Repositories
{

    #region PROCEDURES
    //CREATE PROCEDURE SP_NEQUIPO_JUGADORES
    //@NombreEquipo NVARCHAR(100)
    //AS
    //BEGIN
    //    SELECT j.*
    //    FROM Jugadores j
    //    INNER JOIN Equipos e ON j.EquipoId = e.EquipoId
    //    WHERE e.Nombre = @NombreEquipo
    //END
    //    CREATE VIEW VistaProximosPartidos AS
    //SELECT PP.PARTIDOID, EL.Nombre AS EquipoLocal, EV.Nombre AS EquipoVisitante, EL.Ciudad AS CiudadLocal, EV.Ciudad AS CiudadVisitante, PP.Fecha, PP.PrecioEntrada, PP.PLAZASDISPONIBLES, PP.IMAGENPARTIDO
    //FROM PROXIMOSPARTIDOS PP
    //JOIN EQUIPOS EL ON PP.EquipoLocalId = EL.EquipoId
    //JOIN EQUIPOS EV ON PP.EquipoVisitanteId = EV.EquipoId;

    //    CREATE PROCEDURE SP_OBTENER_GRUPO_JUGADORES
    //    @IndiceInicio INT,
    //    @TamanoPagina INT
    //AS
    //BEGIN
    //    SELECT* FROM(
    //        SELECT ROW_NUMBER() OVER (ORDER BY JUGADORID) AS RowNum, *
    //        FROM JUGADORES
    //    ) AS PaginatedPlayers
    //    WHERE RowNum BETWEEN @IndiceInicio AND @IndiceInicio + @TamanoPagina - 1;
    //END
    #endregion
    public class RepositoryJugadores
    {
        private NbaContext context;
        public RepositoryJugadores(NbaContext context)
        {
            this.context = context;
        }
        public async Task<List<Jugador>> GetAllJugadoresAsync()
        {
            return await this.context.Jugadores.ToListAsync();
        }

        public async Task<List<Jugador>> GetJugadoresByNombreAsync(string nombre)
        {
            var consulta = await this.context.Jugadores.Where(j => j.Nombre.Contains(nombre)).ToListAsync();
            return consulta;
        }

        public async Task<List<Jugador>> GetJugadoresByPosicionAsync(string posicion)
        {
            var consulta = await this.context.Jugadores.Where(j => j.Posicion == posicion).ToListAsync();
            return consulta;
        }
        public async Task<List<Jugador>> GetJugadoresByEquipoAsync(string nombreEquipo)
        {
            // El procedimiento almacenado espera un parámetro de tipo NVARCHAR(100)
            var nombreEquipoParam = new SqlParameter("@NombreEquipo", nombreEquipo);

            // Ejecutar el procedimiento almacenado y obtener los resultados
            var jugadores = await context.Jugadores
                .FromSqlRaw("EXECUTE SP_NEQUIPO_JUGADORES @NombreEquipo", nombreEquipoParam)
                .ToListAsync();

            return jugadores;
        }


        public async Task<Jugador> ObtenerJugadorPorId(int id)
        {
            return await this.context.Jugadores.FindAsync(id);
        }
        public async Task<List<Jugador>> GetGrupoJugadoresAsync(int indiceInicio, int tamanoPagina)
        {
            return await this.context.Jugadores
                .FromSqlRaw("EXEC SP_OBTENER_GRUPO_JUGADORES {0}, {1}", indiceInicio, tamanoPagina)
                .ToListAsync();
        }
        public async Task<int> GetNumeroTotalJugadoresAsync()
        {
            return await this.context.Jugadores.CountAsync();
        }
        public async Task DeleteJugadorAsync(int id)
        {
            var jugador = await context.Jugadores.FindAsync(id);
            if (jugador != null)
            {
                context.Jugadores.Remove(jugador);
                await context.SaveChangesAsync();
            }
        }
        public async Task InsertarJugadorAsync(Jugador nuevoJugador)
        {
            await context.Jugadores.AddAsync(nuevoJugador);
            await context.SaveChangesAsync();
        }

        public async Task ModificarJugadorAsync(Jugador jugadorModificado)
        {
            var jugadorExistente = await context.Jugadores.FindAsync(jugadorModificado.IdJugador);
            jugadorExistente.Nombre = jugadorModificado.Nombre;
            jugadorExistente.EquipoId = jugadorModificado.EquipoId;
            jugadorExistente.Posicion = jugadorModificado.Posicion;
            jugadorExistente.PuntosPorPartido = jugadorModificado.PuntosPorPartido;
            jugadorExistente.RebotesPorPartido = jugadorModificado.RebotesPorPartido;
            jugadorExistente.AsistenciasPorPartido = jugadorModificado.AsistenciasPorPartido;
            jugadorExistente.Imagen = jugadorModificado.Imagen;
            await context.SaveChangesAsync();
        }

    }
}
