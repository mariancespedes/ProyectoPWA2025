using System;
using System.Collections.Generic;

namespace ProyectoPWA2025.DAL;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Contraseña { get; set; } = null!;
}
