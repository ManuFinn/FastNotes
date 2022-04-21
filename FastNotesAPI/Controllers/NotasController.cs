using FastNotesAPI.Models.Notas;
using FastNotesAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasController : ControllerBase
    {
        public progmovilContext Context { get; set; }

        NotasRepository repo;

        public NotasController(progmovilContext context)
        {
            Context = context;
            repo = new NotasRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var notas = repo.GetAll();

            return Ok(notas.Select(x => new
            {
                x.Id,
                x.Titulo,
                x.Contenido
            }));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var notas = repo.Get(id);

            if(notas == null) { return NotFound(); }
            else { return Ok(new
            {
                notas.Id,
                notas.Titulo,
                notas.Contenido
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
                x.Titulo,
                x.Contenido
            }));
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Fastnotesapi n)
        {
            try
            {
                n.Id = 0;
                if(repo.IsValid(n, out List<string> errores))
                {
                    repo.Insert(n);
                    return Ok();
                }
                else { return BadRequest(errores); }
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null) { return StatusCode(500, ex.InnerException.Message); }
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Fastnotesapi n)
        {
            try
            {
                var nota = repo.Get(n.Id);
                if(nota == null) { return NotFound(); }

                if(repo.IsValid(n, out List<string> errores))
                {
                    nota.Titulo = n.Titulo;
                    nota.Contenido = n.Contenido;
                    repo.Update(nota);
                    return Ok();
                }
                else { return BadRequest(errores); }
            }
            catch (Exception ex) {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Fastnotesapi n)
        {
            try
            {
                var nota = repo.Get(n.Id);
                if(nota == null) { return NotFound(); }
                repo.Delete(nota);
                return Ok();
            }
            catch (Exception ex) {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }


    }
}
