using System;
using System.Collections.Generic;

#nullable disable

namespace FastNotesAPI.Models
{
    public partial class ProductoT
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public string TipoProducto { get; set; }
    }
}
