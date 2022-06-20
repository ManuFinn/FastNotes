using System;
using System.Collections.Generic;
using System.Text;

namespace AppIntents.Models
{
    public enum Estado { Agregado, Modificado, Eliminado }

    public class VideoGamesEntity
    {
        public VideogameT VideoGameT { get; set; }

        public Estado Estado { get; set; } 
    }
}
