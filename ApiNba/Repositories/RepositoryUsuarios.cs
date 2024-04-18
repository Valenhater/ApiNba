using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ApiNba.Helpers;
using ApiNba.Models;
using ApiNba.Data;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ApiNba.Repositories
{
    public class RepositoryUsuarios
    {
        private NbaContext context;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryUsuarios(NbaContext context)
        {
            this.context = context;
            string connectionString = "Data Source=sqlvalentin.database.windows.net;Initial Catalog=AZURETAJAMAR;User ID=adminsql;Password=Admin123 ;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
        }


        public async Task<bool> LogInUserAsync(string nombre, string password)
        {
            // Buscar el usuario por su nombre
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre);

            // Verificar si se encontró un usuario y si su nombre, contraseña y salt no son nulos
            if (usuario != null && !string.IsNullOrEmpty(usuario.Nombre) && usuario.Password != null && usuario.Salt != null)
            {
                // Realizar la lógica de inicio de sesión
                string salt = usuario.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = usuario.Password;
                bool response = HelperTools.CompareArrays(temp, passUser);
                return response;
            }
            else
            {
                // Manejar el caso en que no se encuentra el usuario o tiene valores nulos
                if (usuario == null)
                {
                    // Si el usuario no se encuentra, retornar falso
                    return false;
                }
                else
                {
                    // Si alguno de los campos relevantes es nulo, retornar falso
                    return false;
                }
            }
        }

        public Usuario GetUser(string username)
        {
            var usuario = (from u in context.Usuarios
                           where u.Nombre == username
                           select u).FirstOrDefault();

            return usuario;
        }
        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in context.Usuarios
                           select datos;
            return consulta.ToList();
        }
        public bool UsuarioExists(string username)
        {
            var consulta = from u in context.Usuarios
                           where u.Nombre == username
                           select u;

            return consulta.Any();
        }
        public bool EmailExists(string email)
        {
            var consulta = from u in context.Usuarios
                           where u.Email == email
                           select u;

            return consulta.Any();
        }
        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }
        public async Task<Usuario> RegisterUserAsync(string nombre, string password, string email, string nombreCompleto, string direccion)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Nombre = nombre;
            user.Email = email;
            user.Rol = 2;
            user.NombreCompleto = nombreCompleto;
            user.Direccion = direccion;
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperTools.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password = HelperCryptography.EncryptPassword(password, user.Salt);

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public void UpdateUserAsync(int id, string nombre, string email, string nombreCompleto, string direccion)
        {
            var sql = "UPDATE Usuarios SET email = @email, nombre = @nombre";

            // Agregar nombreCompleto solo si se proporciona un valor
            if (!string.IsNullOrEmpty(nombreCompleto))
            {
                sql += ", NombreCompleto = @nombreCompleto";
                this.com.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
            }

            // Agregar direccion solo si se proporciona un valor
            if (!string.IsNullOrEmpty(direccion))
            {
                sql += ", Direccion = @direccion";
                this.com.Parameters.AddWithValue("@direccion", direccion);
            }

            sql += " WHERE idusuario = @id;";

            this.com.Parameters.AddWithValue("@id", id);
            this.com.Parameters.AddWithValue("@email", email);
            this.com.Parameters.AddWithValue("@nombre", nombre);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public async Task<Usuario> GetUserByIdAsync(int id)
        {
            return await context.Usuarios.FindAsync(id);
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await context.Usuarios.FindAsync(id);
            if (user != null)
            {
                context.Usuarios.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
