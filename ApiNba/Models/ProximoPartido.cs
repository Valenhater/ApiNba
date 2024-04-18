using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("PROXIMOSPARTIDOS")]
    public class ProximoPartido
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
        [Column("PRECIOENTRADA")]
        public decimal PrecioEntrada { get; set; }
        [Column("PLAZASDISPONIBLES")]
        public int PlazasDisponible { get; set; }
        [Column("IDCIUDAD")]
        public int IdCiudad { get; set; }
        [Column("IMAGENPARTIDO")]
        public string ImagenPartido { get; set; }
    }
}
