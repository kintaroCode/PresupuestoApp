namespace presupuestoApp.Models;

public class Usuarios
{
    public int UsuariosId { get; set; }
    public string Email { get; set; }

    public string emailNormalizado { get; set; }
    public string PasswordHAsh { get; set; }
}