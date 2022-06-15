using FastNotesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FastNotesAPI.Repositories
{
    public class ProductosRepository : Repository<ProductoT>
    {
        public ProductosRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<ProductoT> GetAll()
        {
            return base.GetAll();
        }
    }
}
