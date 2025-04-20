using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategoria
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }

    public class RepositorioCategoria: IRepositorioCategoria
    {
        private readonly string connectionString;
        public RepositorioCategoria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria)
        {
            using var con = new SqlConnection(connectionString);
            var id = await con.QuerySingleAsync<int>(@"INSERT INTO Categorias(Nombre,TipoOperacionId,UsuarioId)
                                                    VALUES (@Nombre,@TipoOperacionId,@UsuarioId); SELECT SCOPE_IDENTITY();", categoria);

            categoria.Id = id;

        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryAsync<Categoria>(@"SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId,TipoOperacion tipoOperacionId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryAsync<Categoria>(@"SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId AND TipoOperacionId = @tipoOperacionId", new { usuarioId,tipoOperacionId });
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryFirstOrDefaultAsync<Categoria>(@"SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId", new {id,usuarioId});
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync(@"UPDATE Categorias
                                            SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                                            WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var con = new SqlConnection (connectionString);
            await con.ExecuteAsync(@"DELETE Categorias WHERE Id = @Id", new {id});
        }

    }
}
