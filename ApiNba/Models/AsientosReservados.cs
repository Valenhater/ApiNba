using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("AsientosReservados")]
    public class AsientoReservado
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Partido")]
        public int PartidoId { get; set; }

        public int Asiento { get; set; }

        public virtual Partido Partido { get; set; }

        public ModelVistaProximosPartidos ProximosPartidos { get; set; }
    }
}
