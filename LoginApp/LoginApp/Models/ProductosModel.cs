using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginApp.Models
{
    [Table("producto_t")]
    public class ProductosModel
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string nombre_prodcuto { get; set; }

        [NotNull]
        public decimal precio_prodcuto { get; set; }

        [NotNull]
        public string tipo_prodcuto { get; set; }
    }
}
