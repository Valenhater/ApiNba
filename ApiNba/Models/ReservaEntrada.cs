using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("RESERVAENTRADAS")]
    public class ReservaEntrada
    {
        [Key]
        [Column("RESERVAID")]
        public int ReservaId { get; set; }
        [Column("USUARIOID")]
        public int UsuarioId { get; set; }
        [Column("PARTIDOID")]
        public int PartidoId { get; set; }
        [Column("ASIENTO ")]
        public int Asiento { get; set; }
    }
}

