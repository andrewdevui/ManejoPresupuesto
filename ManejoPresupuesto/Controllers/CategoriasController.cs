using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController: Controller
    {
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IServiciousuarios servicioUsuarios;

        public CategoriasController(IRepositorioCategoria repositorioCategoria, IServiciousuarios servicioUsuarios)
        {
            this.repositorioCategoria = repositorioCategoria;
            this.servicioUsuarios = servicioUsuarios;
        }
        
        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categorias = await repositorioCategoria.Obtener(usuarioId, paginacionViewModel);
            var totalCategoria = await repositorioCategoria.Contar(usuarioId);

            var respuestaVM = new PaginacionRespuesta<Categoria>
            {
                Elementos = categorias,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalCategoria,
                BaseUrl = Url.Action()
            };

            return View(respuestaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            categoria.UsuarioId = usuarioId;

            await repositorioCategoria.Crear(categoria);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategoria.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categoriaExiste = await repositorioCategoria.ObtenerPorId(categoria.Id, usuarioId);

            if (categoriaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategoria.Actualizar(categoria);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategoria.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategoria.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategoria.Borrar(id);
            return RedirectToAction("Index");
        }

    }
}
