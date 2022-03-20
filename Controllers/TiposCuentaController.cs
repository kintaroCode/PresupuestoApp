using Microsoft.AspNetCore.Mvc;
using presupuestoApp.Models;
using presupuestoApp.Servicios;

namespace presupuestoApp.Controllers;

public class TiposCuentaController:Controller
{
    private readonly IRepositorioTiposCuenta _repositorioTiposCuenta;
    private readonly IServiciosUsuarios _servicesUsuarios;

    public TiposCuentaController(IRepositorioTiposCuenta repositorioTiposCuenta,
                                 IServiciosUsuarios servicesUsuarios)
    {
        _repositorioTiposCuenta=repositorioTiposCuenta;
        _servicesUsuarios=servicesUsuarios;
    }

    public async Task<IActionResult> Index()
    {
          var usuarioId=_servicesUsuarios.ObtenerUsuariosId();
          var tiposCuenta = await _repositorioTiposCuenta.Obtener(usuarioId);
          
          return View(tiposCuenta);     
    }

    public IActionResult Crear()
    {        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(TiposCuenta tiposCuenta)
    {
        if (!ModelState.IsValid)
        {
            return View(tiposCuenta);
        }
        tiposCuenta.UsuarioId=_servicesUsuarios.ObtenerUsuariosId();

        var yaExisteTiposCuenta= await _repositorioTiposCuenta.Existe(tiposCuenta.Nombre, tiposCuenta.UsuarioId);
        if (yaExisteTiposCuenta)
        {
            ModelState.AddModelError(nameof(tiposCuenta.Nombre), $"el Tipo {tiposCuenta.Nombre} ya existe");
            return View(tiposCuenta);
        }
        await _repositorioTiposCuenta.Crear(tiposCuenta);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Editar(int TiposCuentaId)
    {
        var usuarioId= _servicesUsuarios.ObtenerUsuariosId();
        var tipoCuenta= await _repositorioTiposCuenta.ObtenerPorId(TiposCuentaId, usuarioId);

        if (tipoCuenta is null)
        {
            return RedirectToAction("NoEncontrado", "Home");
        }
        return View(tipoCuenta);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(TiposCuenta tiposCuenta)
    {
        var usuarioId=_servicesUsuarios.ObtenerUsuariosId();
        var tipoCuentaExiste=_repositorioTiposCuenta.ObtenerPorId(tiposCuenta.TiposCuentaId, usuarioId);
        if (tipoCuentaExiste is null)
        {
            return RedirectToAction("NoEncontrado", "Home");
        }

        await _repositorioTiposCuenta.Actualizar(tiposCuenta);
        return RedirectToAction("Index");

    }
    
    [HttpGet]
    public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
    {
        var usuarioId=_servicesUsuarios.ObtenerUsuariosId();
        var yaExisteTiposCuenta=await _repositorioTiposCuenta.Existe(nombre, usuarioId);

        if (yaExisteTiposCuenta)
        {
            return Json($"El tipo cuenta {nombre} ya existe");
            
        }
        return Json(true);
    }
}