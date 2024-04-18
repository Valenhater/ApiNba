using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNba.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("idusuario")]
        public int IdUsuario { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("passwd")]
        public byte[] Password { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [ForeignKey("Rol")]
        public int Rol { get; set; }
        [Column("NombreCompleto")]
        public string NombreCompleto { get; set; }
        [Column("Direccion")]
        public string Direccion { get; set; }
    }
}
