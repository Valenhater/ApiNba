using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("VistaEntradasReservadas")]
    public class VistaEntradasReservadas
    {
        [Key]
        [Column("ReservaId")]
        public int ReservaId { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("PartidoId")]
        public int PartidoId { get; set; }

        [Column("Asiento")]
        public int Asiento { get; set; }

        [Column("FechaPartido")]
        public DateTime FechaPartido { get; set; }

        [Column("EquipoLocalId")]
        public int EquipoLocalId { get; set; }

        [Column("EquipoVisitanteId")]
        public int EquipoVisitanteId { get; set; }

        [Column("PrecioEntrada")]
        public decimal PrecioEntrada { get; set; }

        [Column("PLAZASDISPONIBLES")]
        public int PlazasDisponibles { get; set; }

        [Column("IdCiudad")]
        public int IdCiudad { get; set; }

        [Column("ImagenPartido")]
        public string ImagenPartido { get; set; }
    }
}
