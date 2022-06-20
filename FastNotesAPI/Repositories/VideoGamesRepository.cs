using FastNotesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastNotesAPI.Repositories
{
    public class VideoGamesRepository : Repository<VideogameT>
    {
        public VideoGamesRepository(DbContext context) : base(context)
        {
        }

        public override void Insert(VideogameT entity)
        {
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Insert(entity);
        }

        public override IEnumerable<VideogameT> GetAll()
        {
            return base.GetAll().Where(x => x.Eliminado == 0).OrderByDescending(x => x.Timestamp);
        }

        public IEnumerable<VideogameT> GetAllSinceDate(DateTime timestamp)
        {
            var cambiados = base.GetAll().Where((x) => x.Eliminado == 0 && x.Timestamp > timestamp);
            var eliminados = base.GetAll().Where((x) => x.Eliminado == 1 && x.Timestamp > timestamp)
                .Select(x => new VideogameT { Id = x.Id });

            return cambiados.Concat(eliminados);
        }

        public override void Update(VideogameT entity)
        {
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Update(entity);
        }

        public override void Delete(VideogameT entity)
        {
            if (entity.Eliminado == 0)
            {
                entity.Timestamp = DateTime.Now.ToMexicoTime();
                entity.Eliminado = 1;
                base.Update(entity);
            }

            int TTL = 64;

            var fechaEliminar = DateTime.Now.ToMexicoTime().Subtract(TimeSpan.FromDays(TTL));

            var porEliminar = base.GetAll().Where(x => x.Eliminado == 1 && x.Timestamp < fechaEliminar);

            foreach (var item in porEliminar)
            {
                Context.Remove(item);
            }
            Context.SaveChanges();
        }

        public override bool IsValid(VideogameT entity, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(entity.NombreVg))
            {
                validationErrors.Add("El videojuego no tiene nombre...");
            }
            if (entity.FechaSalidaVg == DateTime.UtcNow)
            {
                validationErrors.Add("Debe de seleccionar una fecha valida...");
            }
            return validationErrors.Count == 0;
        }

    }
}
