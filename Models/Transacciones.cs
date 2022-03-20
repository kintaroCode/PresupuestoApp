namespace presupuestoApp.Models;

public class Transacciones
{
     public int TransaccionesId { get; set; }
     public int UsuariosId { get; set; }
     public DateTime FechaTransaccion {get;set;}
     public  double Monto { get; set; }
     public int TipoOperacion { get; set; }
     public string Nota { get; set; }
    public int CuentasId { get; set; }
    public int CategoriasIdMyProperty { get; set; }

}