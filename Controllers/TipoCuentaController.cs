using Microsoft.AspNetCore.Mvc;
using presupuestoApp.Models;
using presupuestoApp.Servicios;

namespace presupuestoApp.Controllers;

public class TipoCuentaController:Controller
{
    private readonly IRepositorioTipoCuenta _repositorioTipoCuenta;
    private readonly IServiciosUsuarios _servicesUsuarios;

    public TipoCuentaController(IRepositorioTipoCuenta repositorioTipoCuenta,
                                 IServiciosUsuarios servicesUsuarios)
    {
        _repositorioTipoCuenta=repositorioTipoCuenta;
        _servicesUsuarios=servicesUsuarios;
    }

    public async Task<IActionResult> Index()
    {
          var usuarioId=_servicesUsuarios.ObtenerUsuariosId();
          var tipoCuenta = await _repositorioTipoCuenta.Obtener(usuarioId);
          
          return View(tipoCuenta);     
    }

    public IActionResult Crear()
    {        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
    {
        if (!ModelState.IsValid)
        {
            return View(tipoCuenta);
        }
        tipoCuenta.UsuarioId=_servicesUsuarios.ObtenerUsuariosId();

        var yaExisteTipoCuenta= await _repositorioTipoCuenta.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
        if (yaExisteTipoCuenta)
        {
            ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"el Tipo {tipoCuenta.Nombre} ya existe");
            return View(tipoCuenta);
        }
        await _repositorioTipoCuenta.Crear(tipoCuenta);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Editar(int Id)
    {
        var usuarioId= _servicesUsuarios.ObtenerUsuariosId();
        var tipoCuenta= await _repositorioTipoCuenta.ObtenerPorId(Id, usuarioId);

        if (tipoCuenta is null)
        {
            return RedirectToAction("NoEncontrado", "Home");
        }
        return View(tipoCuenta);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
    {
        var usuarioId = _servicesUsuarios.ObtenerUsuariosId();
        var tipoCuentaExiste = _repositorioTipoCuenta.ObtenerPorId(tipoCuenta.Id, usuarioId);
        if (tipoCuentaExiste is not null)
        {
            await _repositorioTipoCuenta.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }
        return RedirectToAction("NoEncontrado", "Home");

    }

    [HttpGet]
    public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
    {
        var usuarioId=_servicesUsuarios.ObtenerUsuariosId();
        var yaExisteTipoCuenta=await _repositorioTipoCuenta.Existe(nombre, usuarioId);

        if (yaExisteTipoCuenta)
        {
            return Json($"El tipo cuenta {nombre} ya existe");
            
        }
        return Json(true);
    }
}