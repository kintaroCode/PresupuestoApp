namespace presupuestoApp.Servicios;

public interface IServiciosUsuarios
{
    int ObtenerUsuariosId();
}
public class ServiciosUsuarios:IServiciosUsuarios
{
    public int ObtenerUsuariosId()
    {
        return 1;
    }
}