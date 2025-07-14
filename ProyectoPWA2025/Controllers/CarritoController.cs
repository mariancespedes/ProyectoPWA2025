using Microsoft.AspNetCore.Mvc;
using ProyectoPWA2025.Helpers;
using System.Diagnostics;
using ProyectoPWA2025.DAL;


namespace ProyectoPWA2025.Controllers
{
    public class CarritoController : Controller
    {
        public List<Evento> eventos = new List<Evento>();
        public List<Carrito> item = new List<Carrito>();
        public decimal total = 0;
        int Contar = 0;
        private readonly WebExtendedContext _CaContext;
        public CarritoController(WebExtendedContext _context)
        {
            _CaContext = _context;
        }
        public IActionResult Index()
        {
            int cantidad;
            var cart = SessionHelper.GetObjectFromJson<List<Carrito>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cantidad = 0;
            }
            else
            {
                cantidad = cart.Count;
            }
            TempData["Contar"] = cantidad;
            eventos = _CaContext.Eventos.ToList();
            return View(eventos);

        }
        public IActionResult Carrito()
        {
            var miCarrito = ProyectoPWA2025.Helpers.SessionHelper.GetObjectFromJson<List<Carrito>>(HttpContext.Session, "cart");
            if (miCarrito == null)
            {
                return RedirectToAction("Index", "Carrito");
            }
            else
            {
                return View(miCarrito);
            }
        }
        public int ContarItems(List<Carrito> items)
        {
            int cantidad = items.Count();
            return cantidad;
        }
        private int Exist(List<Carrito> cart, string id)
        {
            for (int i = 0; i < cart.Count; i++) 
            {
                if (cart[i].IdEvento.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        [HttpGet]
        public IActionResult Cart(string id)
        {
            var prod = _CaContext.Eventos.Find(id);
            var cart = ProyectoPWA2025.Helpers.SessionHelper.GetObjectFromJson<List<Carrito>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart=new List<Carrito>();
                cart.Add(new Carrito()
                {
                    IdEventoNavigation = prod,
                    Cantidad = 1

                });
                TempData["Contar"]=ContarItems(cart);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index=Exist(cart, id);
                if (index == -1)
                {
                    cart.Add(new Carrito()
                    {
                        IdEventoNavigation = prod,
                        Cantidad = 1
                    });
                }
                else
                {
                    var newCantidad = cart[index].Cantidad + 1;
                    cart[index].Cantidad = newCantidad;
                }
            TempData["Contar"]=ContarItems(cart);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index", "Carrito");
        }
        [HttpGet]
        public IActionResult Quitar(string id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Carrito>>(HttpContext.Session, "cart");
            int index = Exist(cart, id);
            cart.RemoveAt(index);
            TempData["Contar"] = ContarItems(cart);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index", "Carrito");
        }
    }
}
