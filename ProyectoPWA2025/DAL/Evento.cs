using System;
using System.Collections.Generic;

namespace ProyectoPWA2025.DAL;

public partial class Evento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public decimal Precio { get; set; }

    public int Cupo { get; set; }

    public string Foto { get; set; } = null!;
}
