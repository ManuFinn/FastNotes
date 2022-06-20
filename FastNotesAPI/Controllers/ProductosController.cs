using FastNotesAPI.Models;
using FastNotesAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : Controller
    {
        public itesrcne_181g0250Context Context { get; set; }

        ProductosRepository repo;

        public ProductosController(itesrcne_181g0250Context context)
        {
            Context = context;
            repo = new ProductosRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var productos = repo.GetAll();

            return Ok(productos);
        }


    }
}
