namespace ApiNba.Models
{
    public class JugadoresViewModel
    {
        public List<Jugador> Jugadores { get; set; }
        public List<Equipo> Equipos { get; set; }

        // Propiedades para paginación
        public string Nombre { get; set; }
        public string Posicion { get; set; }
        public string Equipo { get; set; }
        public int TotalJugadores { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalJugadores / TamañoPagina);
        public int TamañoPagina { get; set; } = 10; // Tamaño de página predeterminado
    }
}
