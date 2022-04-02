using System.Data.SqlClient;
using Dapper;
using presupuestoApp.Models;

namespace presupuestoApp.Servicios;

public interface IRepositorioTipoCuenta
{
    Task Actualizar(TipoCuenta tipoCuenta);
    Task Crear(TipoCuenta tipoCuenta);
    Task<bool> Existe(string nombre, int usuarioId);
    Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
    Task<TipoCuenta> ObtenerPorId(int Id, int usuarioId);
}

public class RepositorioTipoCuenta : IRepositorioTipoCuenta
{
    private readonly string Conex;

    public RepositorioTipoCuenta(IConfiguration configuration)
    {
        Conex =configuration.GetConnectionString("ConexDb");
    }
    public async Task Crear(TipoCuenta tipoCuenta )
    {
        using var connection=new SqlConnection(Conex);

        var id=await connection.QuerySingleAsync<int>($@"INSERT INTO TipoCuenta (nombre, usuarioId,orden)
                                            values(@Nombre, @UsuarioId,0);
                                            SELECT SCOPE_IDENTITY();", tipoCuenta); 
        tipoCuenta.Id=id;

    }

    public async Task<bool> Existe(string nombre, int usuarioId)
    {
        using var connection=new SqlConnection(Conex);

        var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TIPOCUENTA WHERE NOMBRE=@NOMBRE AND USUARIOID= @USUARIOID;" , new {nombre, usuarioId});

        return existe == 1;

    }
    public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
    {
        using var connection=new SqlConnection(Conex);

        return await connection.QueryAsync<TipoCuenta>(@"select Id, Nombre, Orden, UsuarioId from tipoCuenta where UsuarioId=@usuarioId", new {usuarioId});

    }

    public async Task Actualizar(TipoCuenta tipoCuenta)
    {
        using var connection= new SqlConnection(Conex);
        
        await connection.ExecuteAsync(@"UPDATE TipoCuenta SET Nombre=@Nombre
                                        WHERE Id=@Id", tipoCuenta);
    }

    public async Task<TipoCuenta> ObtenerPorId(int Id, int usuarioId)
    {
        using var connection= new SqlConnection(Conex);
        return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"
                                                            SELECT Id, Nombre,Orden, UsuarioId
                                                            FROM TipoCuenta WHERE Id=@Id AND UsuarioId=@usuarioId", new { Id, usuarioId});

                                                                    
    }
}    