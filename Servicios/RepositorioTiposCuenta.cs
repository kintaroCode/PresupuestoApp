using System.Data.SqlClient;
using Dapper;
using presupuestoApp.Models;

namespace presupuestoApp.Servicios;

public interface IRepositorioTiposCuenta
{
    Task Actualizar(TiposCuenta tiposCuenta);
    Task Crear(TiposCuenta tiposCuenta);
    Task<bool> Existe(string nombre, int usuarioId);
    Task<IEnumerable<TiposCuenta>> Obtener(int usuarioId);
    Task<TiposCuenta> ObtenerPorId(int TiposCuentaId, int usuarioId);
}

public class RepositorioTiposCuenta : IRepositorioTiposCuenta
{
    private readonly string Conex;

    public RepositorioTiposCuenta(IConfiguration configuration)
    {
        Conex =configuration.GetConnectionString("ConexDb");
    }
    public async Task Crear(TiposCuenta tiposCuenta )
    {
        using var connection=new SqlConnection(Conex);

        var id=await connection.QuerySingleAsync<int>($@"INSERT INTO TipoCuenta (nombre, usuarioId,orden)
                                            values(@Nombre, @UsuarioId,0);
                                            SELECT SCOPE_IDENTITY();", tiposCuenta); 
        tiposCuenta.UsuarioId=id;

    }

    public async Task<bool> Existe(string nombre, int usuarioId)
    {
        using var connection=new SqlConnection(Conex);

        var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TIPOCUENTA WHERE NOMBRE=@NOMBRE AND USUARIOID= @USUARIOID;" , new {nombre, usuarioId});

        return existe == 1;

    }
    public async Task<IEnumerable<TiposCuenta>> Obtener(int usuarioId)
    {
        using var connection=new SqlConnection(Conex);

        return await connection.QueryAsync<TiposCuenta>(@"select TipoCuentaId, Nombre, Orden from tipoCuenta where usuarioId=@usuarioId", new {usuarioId});

    }

    public async Task Actualizar(TiposCuenta tiposCuenta)
    {
        using var connection= new SqlConnection(Conex);
        
        await connection.ExecuteAsync(@"UPDATE TipoCuenta SET Nombre=@Nombre
                                        WHERE TipoCuentaId=@tipoCuentaId", tiposCuenta);
    }

    public async Task<TiposCuenta> ObtenerPorId(int TiposCuentaId, int usuarioId)
    {
        using var connection= new SqlConnection(Conex);
        return await connection.QueryFirstOrDefaultAsync<TiposCuenta>(@"
                                                            SELECT tipoCuentaId, Nombre,Orden
                                                            FROM TipoCuenta WHERE TipoCuentaId=@TiposCuentaId AND UsuarioId=@usuarioId", new { TiposCuentaId, usuarioId});

                                                                    
    }
}    