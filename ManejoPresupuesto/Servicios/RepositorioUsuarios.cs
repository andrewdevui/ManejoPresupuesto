using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task Actualizar(Usuario usuario);
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios:IRepositorioUsuarios
    {
        public readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuracion)
        {
            connectionString = configuracion.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var con = new SqlConnection(connectionString);
            var usuarioId = await con.QuerySingleAsync<int>("INSERT INTO Usuarios(Email,EmailNormalizado,PasswordHash)\r\nVALUES (@Email,@EmailNormalizado,@PasswordHash); SELECT SCOPE_IDENTITY();",usuario);
            await con.ExecuteAsync("CrearDatosUsuarioNuevo",new {usuarioId},commandType: System.Data.CommandType.StoredProcedure);
            
            return usuarioId;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var con = new SqlConnection(connectionString);
            var usuario = await con.QuerySingleOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE EmailNormalizado = @emailNormalizado", new { emailNormalizado });
            return usuario;
        }

        public async Task Actualizar(Usuario usuario)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync(@"UPDATE Usuarios SET PasswordHash = @PasswordHash WHERE Id = @Id", usuario);

        }
    }
}
