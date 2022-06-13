using System;
using System.Collections.Generic;

#nullable disable

namespace FastNotesAPI.Models
{
    public partial class PuntuacionT
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Puntos { get; set; }
    }
}
