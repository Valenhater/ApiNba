using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("PARTIDOS")]
    public class Partido
    {
        [Key]
        [Column("PARTIDOID")]
        public int IdPartido { get; set; }
        [Column("EQUIPOLOCALID")]
        public int EquipoLocalId { get; set; }
        [Column("EQUIPOVISITANTEID")]
        public int EquipoVisitanteId { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("PUNTOSLOCAL")]
        public int PuntosLocal { get; set; }
        [Column("PUNTOSVISITANTE")]
        public int PuntosVisitante { get; set; }
        [ForeignKey("EquipoLocalId")]
        public Equipo EquipoLocal { get; set; }

        [ForeignKey("EquipoVisitanteId")]
        public Equipo EquipoVisitante { get; set; }
    }
}
