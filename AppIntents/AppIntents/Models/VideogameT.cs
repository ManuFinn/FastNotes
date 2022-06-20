using SQLite;
using System;
using System.Collections.Generic;



namespace AppIntents.Models
{
    [Table("videogame_t")]
    public class VideogameT
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string NombreVg { get; set; }

        public string DescripcionVg { get; set; }

        [NotNull]
        public DateTime FechaSalidaVg { get; set; }

        public string PortadaVg { get; set; }
    }
}
