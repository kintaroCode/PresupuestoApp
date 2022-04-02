using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace presupuestoApp.Models;

public class TipoCuenta
{
    public int Id { get; set; }
    
    [Required(ErrorMessage ="El Nombre es requerido")]   
    [StringLength(maximumLength:50,MinimumLength =3, ErrorMessage ="el {0} debe tener entre 3 y 50 caracteres")] 
    [Display(Name ="Nombre del tipo de cuenta")]  
    [Remote(action:"VerificarExisteTipoCuenta", controller:"TiposCuenta")]
    public string Nombre { get; set; }
    public int UsuarioId { get; set; }
    public int Orden { get; set; }
}