using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borra(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTransacciones: IRepositorioTransacciones
    {
        private readonly string connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var con = new SqlConnection(connectionString);
            var id = await con.QuerySingleAsync<int>("Transacciones_Insertar", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota
            }, commandType:System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }


        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryAsync<Transaccion>(@"SELECT T.Id, T.Monto, T.FechaTransaccion, CA.Nombre AS Categoria, CT.Nombre as Cuenta, CA.TipoOperacionId 
                                                        FROM Transacciones AS T
                                                        INNER JOIN Categorias CA ON CA.Id = t.CategoriaId
                                                        INNER JOIN Cuentas CT ON CT.Id = T.CuentaId
                                                        WHERE T.CuentaId = @CuentaId AND T.UsuarioId = @UsuarioId 
                                                        AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin", modelo);
        }

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior,int cuentaAnteriorId)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync("Transacciones_Actualizar",new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId
            },commandType:System.Data.CommandType.StoredProcedure);

        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryFirstOrDefaultAsync<Transaccion>(@"SELECT tr.*, cat.TipoOperacionId
                                                        FROM Transacciones as tr
                                                        INNER JOIN Categorias cat
                                                        ON cat.Id = tr.CategoriaId
                                                        WHERE tr.Id = @Id AND tr.UsuarioId = @UsuarioId", new {id,usuarioId});
        }

        public async Task Borra(int id)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync("Transacciones_Borrar", new {id}, commandType: System.Data.CommandType.StoredProcedure);
        }

    }
}
