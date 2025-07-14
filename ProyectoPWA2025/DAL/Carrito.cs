using System;
using System.Collections.Generic;

namespace ProyectoPWA2025.DAL;

public partial class Carrito
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdEvento { get; set; }

    public int Cantidad { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
