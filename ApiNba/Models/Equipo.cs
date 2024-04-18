using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("EQUIPOS")]
    public class Equipo
    {
        [Key]
        [Column("EQUIPOID")]
        public int IdEquipo { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("CIUDAD")]
        public string Ciudad { get; set; }
        [Column("FUNDACION")]
        public DateTime Fundacion { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("IMGEQUIPOS")]
        public string ImagenFondo { get; set; }
    }
}
