using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiNba.Models
{
    [Table("VistaProximosPartidos")]
    public class ModelVistaProximosPartidos
    {
        [Key]
        [Column("PARTIDOID")]
        public int IdPartido { get; set; }

        [Column("EquipoLocal")]
        public string EquipoLocal { get; set; }

        [Column("EquipoVisitante")]
        public string EquipoVisitante { get; set; }

        [Column("CiudadLocal")]
        public string CiudadLocal { get; set; }

        [Column("CiudadVisitante")]
        public string CiudadVisitante { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("PrecioEntrada")]
        public decimal PrecioEntrada { get; set; }

        [Column("PLAZASDISPONIBLES")]
        public int PlazasDisponibles { get; set; }

        [Column("ImagenPartido")]
        public string ImagenPartido { get; set; }
    }
}
