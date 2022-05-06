using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Models
{
    public enum Estado { Agregado, Modificado, Eliminado}

    public class NotasEntity
    {
        public Notas Nota { get; set; }

        public Estado Estado { get; set; }
    }
}
