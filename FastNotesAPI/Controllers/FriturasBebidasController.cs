using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlumnosController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(new[] { 
                "Doritos", 
                "Sabritas", 
                "Takis", 
                "Pepsi Cola", 
                "Coca-Cola", 
                "Dr. Pepper", 
                "Rancheritos", 
                "Chetos" });
        }

    }
}
