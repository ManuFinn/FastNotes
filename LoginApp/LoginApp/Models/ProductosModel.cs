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
        public string NombreProducto { get; set; }

        [NotNull]
        public decimal PrecioProducto { get; set; }

        [NotNull]
        public string TipoProducto { get; set; }
    }
}
