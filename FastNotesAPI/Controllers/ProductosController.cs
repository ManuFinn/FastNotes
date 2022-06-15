using FastNotesAPI.Models;
using FastNotesAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var notas = repo.GetAll();

            return Ok(notas);
        }


    }
}
