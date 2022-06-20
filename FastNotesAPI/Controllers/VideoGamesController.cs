using FastNotesAPI.Models;
using FastNotesAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : ControllerBase
    {
        public itesrcne_181g0250Context Context { get; set; }

        VideoGamesRepository repo;

        public VideoGamesController(itesrcne_181g0250Context context)
        {
            Context = context;
            repo = new VideoGamesRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vg = repo.GetAll();
            return Ok(vg.Select(x => new
            {
                x.Id,
                x.NombreVg,
                x.DescripcionVg,
                x.PortadaVg,
                x.FechaSalidaVg
            }));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var vg = repo.Get(id);
            if (vg == null) { return NotFound(); }
            else
            {
                return Ok(new
                {
                    vg.Id,
                    vg.NombreVg,
                    vg.DescripcionVg,
                    vg.PortadaVg,
                    vg.FechaSalidaVg
                });
            }
        }

        [HttpPost("sincronizar")]
        public IActionResult Post([FromBody] DateTime fecha)
        {
            var notas = repo.GetAllSinceDate(fecha.ToMexicoTime());
            return Ok(notas.Select(x => new
            {
                x.Id,
                x.NombreVg,
                x.DescripcionVg,
                x.PortadaVg,
                x.FechaSalidaVg
            }));
        }

        [HttpPost]
        public IActionResult Post([FromBody] VideogameT videogame)
        {
            try
            {
                videogame.Id = 0;
                if (repo.IsValid(videogame, out List<string> errores))
                {
                    repo.Insert(videogame);
                    return Ok();
                }
                else { return BadRequest(errores); }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) { return StatusCode(500, ex.InnerException.Message); }
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] VideogameT videogame)
        {
            try
            {
                var vg = repo.Get(videogame.Id);
                if (vg == null) { return NotFound(); }

                if (repo.IsValid(videogame, out List<string> errores))
                {
                    vg.NombreVg = videogame.NombreVg;
                    vg.DescripcionVg = videogame.DescripcionVg;
                    vg.PortadaVg = videogame.PortadaVg;
                    vg.FechaSalidaVg = videogame.FechaSalidaVg;
                    repo.Update(vg);
                    return Ok();
                }
                else { return BadRequest(errores); }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] VideogameT videogame)
        {
            try
            {
                var vg = repo.Get(videogame.Id);
                if (vg == null) { return NotFound(); }
                repo.Delete(vg);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
