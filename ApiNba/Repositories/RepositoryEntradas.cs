using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ApiNba.Data;
using ApiNba.Models;
using System.Data;

namespace ApiNba.Repositories
{
    public class RepositoryEntradas
    {
        private NbaContext context;

        public RepositoryEntradas(NbaContext context)
        {
            this.context = context;
        }
        public async Task<List<ModelVistaProximosPartidos>> GetProximosPartidosAsync()
        {
            return  this.context.VistaProximosPartidos.ToList();
        }
        public async Task<ModelVistaProximosPartidos> FindPartidoAsync(int idPartido)
        {
            return await this.context.VistaProximosPartidos.FirstOrDefaultAsync(z => z.IdPartido == idPartido);
        }

        public async Task<List<ModelVistaProximosPartidos>> GetFavoritosAsync(List<int> ids)
        {
            var consulta = from datos in this.context.VistaProximosPartidos where ids.Contains(datos.IdPartido) select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return await consulta.ToListAsync();
            }
        }
        public async Task<ReservaEntrada> ReservarEntradaAsync(int usuarioId, int partidoId, int asiento)
        {
            // Buscar el partido
            var partido = await this.context.ProximosPartidos.FirstOrDefaultAsync(p => p.IdPartido == partidoId);

            // Verificar si el partido existe y hay plazas disponibles
            if (partido != null && partido.PlazasDisponible > 0)
            {
                // Restar una plaza disponible
                partido.PlazasDisponible--;

                // Crear la reserva de entrada
                var reserva = new ReservaEntrada
                {
                    UsuarioId = usuarioId,
                    PartidoId = partidoId,
                    Asiento = asiento
                };

                // Guardar los cambios en la base de datos
                this.context.ReservaEntradas.Add(reserva);
                await this.context.SaveChangesAsync();

                return reserva;
            }
            else
            {
                // En caso de que el partido no exista o no haya plazas disponibles, retornar null
                return null;
            }
        }

        public async Task<List<ReservaEntrada>> GetReservasEntradasAsync()
        {
            return await context.ReservaEntradas
                .Include(r => r.Usuario)
                .Include(r => r.Partido)
                .ToListAsync();
        }
        public async Task<bool> EliminarReservaEntradaAsync(int reservaId)
        {
            var reserva = await context.ReservaEntradas.FindAsync(reservaId);
            if (reserva != null)
            {
                context.ReservaEntradas.Remove(reserva);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
