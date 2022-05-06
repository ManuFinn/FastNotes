using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Models
{
    [Table("fastnotesapi")]
    public class Notas
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }
        public string Titulo { get; set; }
        [NotNull]
        public string Contenido { get; set; }
    }
}
