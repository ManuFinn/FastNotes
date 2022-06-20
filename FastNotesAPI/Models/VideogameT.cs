using System;
using System.Collections.Generic;

#nullable disable

namespace FastNotesAPI.Models
{
    public partial class VideogameT
    {
        public int Id { get; set; }
        public string NombreVg { get; set; }
        public string DescripcionVg { get; set; }
        public DateTime FechaSalidaVg { get; set; }
        public string PortadaVg { get; set; }
        public ulong Eliminado { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
