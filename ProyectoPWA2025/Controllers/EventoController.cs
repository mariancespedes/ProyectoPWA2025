using Microsoft.AspNetCore.Mvc;
using ProyectoPWA2025.DAL;
using ProyectoPWA2025.Models;

namespace ProyectoPWA2025.Controllers
{
    public class EventoController : Controller
    {
        private readonly WebExtendedContext _DbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EventoController(WebExtendedContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            _DbContext = _context;
            this.webHostEnvironment = _webHostEnvironment;
        }
        public string UploadFile(EventoVM modeloEvento)
        {
            string nombreArchivo = null;
            if (modeloEvento.FotoPath != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "images");
                nombreArchivo = Guid.NewGuid().ToString() + "-" + modeloEvento.FotoPath.FileName;
                string rutaArchivo = Path.Combine(uploadDir, nombreArchivo);
                using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    modeloEvento.FotoPath.CopyTo(fileStream);
                }
            }
            return nombreArchivo;
        }
        public IActionResult Index()
        {
            List<Evento> lista = _DbContext.Eventos.ToList();
            return View(lista);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(EventoVM modeloEvento)
        {
            string NombreArchivo = UploadFile(modeloEvento);
            Evento evento = new Evento()
            {
                Id = modeloEvento.oEvento.Id,
                Nombre = modeloEvento.oEvento.Nombre,
                Fecha = modeloEvento.oEvento.Fecha,
                Descripcion = modeloEvento.oEvento.Descripcion,
                Precio = modeloEvento.oEvento.Precio,
                Cupo = modeloEvento.oEvento.Cupo,
                Foto = NombreArchivo
            };
            _DbContext.Eventos.Add(evento);
            _DbContext.SaveChanges();
            return RedirectToAction("Index", "Evento");
        }
        public Evento EncontrarEvento(int id)
        {
            Evento evento = _DbContext.Eventos.Find(id);
            return evento;
        }
        [HttpGet]
        public IActionResult Actualizar(int idEvento)
        {
            Evento evento= EncontrarEvento(idEvento);
            TempData["urlphoto"] = evento.Foto;
            return View(evento);
        }

        [HttpPost]
        public IActionResult Actualizar(Evento modeloEvento, IFormFile NewFoto)
        {
            Evento evento = new Evento()
            {
                Id = modeloEvento.Id,
                Nombre = modeloEvento.Nombre,
                Fecha = modeloEvento.Fecha,
                Descripcion = modeloEvento.Descripcion,
                Precio = modeloEvento.Precio,
                Cupo = modeloEvento.Cupo,
            };
            if (NewFoto != null)
            {
                EventoVM vmEvento = new EventoVM();
                vmEvento.oEvento = evento;
                vmEvento.FotoPath = NewFoto;
                string NombreArchivo = UploadFile(vmEvento);
                evento.Foto = NombreArchivo;
            }
            else
            {
                evento.Foto = TempData["urlphoto"].ToString();
            }
            _DbContext.Eventos.Update(evento);
            _DbContext.SaveChanges();
            return RedirectToAction("Index","Evento");
        }
        [HttpGet]
        public IActionResult Eliminar(int idEvento)
        {
            Evento evento= EncontrarEvento(idEvento);
            return View(evento);
        }

        [HttpPost]
        public IActionResult Eliminar(Evento modeloEvento)
        {
            _DbContext.Eventos.Remove(modeloEvento);
            _DbContext.SaveChanges();
            return View("Index","Evento");
        }


    }
}
