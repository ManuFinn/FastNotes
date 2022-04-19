using System;
using System.Collections.Generic;

#nullable disable

namespace FastNotesAPI.Models.Notas
{
    public partial class Fastnotesapi
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public ulong Eliminado { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
