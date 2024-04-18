using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("JUGADORES")]
    public class Jugador
    {
        [Key]
        [Column("JUGADORID")]
        public int IdJugador { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("EQUIPOID")]
        public int EquipoId { get; set; }
        [Column("POSICION")]
        public string Posicion { get; set; }
        [Column("PUNTOSPORPARTIDO")]
        public decimal PuntosPorPartido { get; set; }
        [Column("REBOTESPORPARTIDO")]
        public decimal RebotesPorPartido { get; set; }
        [Column("ASISTENCIASPORPARTIDO")]
        public decimal AsistenciasPorPartido { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
    }
}
