using Microsoft.EntityFrameworkCore;
using ApiNba.Models;

namespace ApiNba.Data
{
    public class NbaContext : DbContext
    {
        public NbaContext(DbContextOptions<NbaContext> options) : base(options) { }
        

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Jugador> Jugadores { get; set;}
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<ProximoPartido> ProximosPartidos { get; set; }
        public DbSet<ReservaEntrada> ReservaEntradas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ModelVistaProximosPartidos> VistaProximosPartidos { get; set; }

    }
}
