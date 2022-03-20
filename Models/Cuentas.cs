namespace presupuestoApp.Models;

public class Cuentas
{
    public int CuentasId { get; set; }
    public string Nombre { get; set; }
    public int TiposCuentaId { get; set; }
    public double Balance { get; set; }
    public string Descripcion { get; set; }
}