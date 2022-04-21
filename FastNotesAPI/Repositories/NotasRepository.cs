using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FastNotesAPI.Models.Notas;

namespace FastNotesAPI.Repositories
{
    public class NotasRepository : Repository<Fastnotesapi>
    {
        public NotasRepository(DbContext context) : base(context)
        {
        }

        public override void Insert(Fastnotesapi entity)
        {
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Insert(entity);
        }

        public override IEnumerable<Fastnotesapi> GetAll()
        {
            return base.GetAll().Where(x => x.Eliminado == 0).OrderBy(x => x.Timestamp);
        }

        public IEnumerable<Fastnotesapi> GetAllSinceDate(DateTime timestamp)
        {
            var cambiados = base.GetAll().Where((x) => x.Eliminado == 0 && x.Timestamp > timestamp);
            var eliminados = base.GetAll().Where((x) => x.Eliminado == 1 && x.Timestamp > timestamp)
                .Select(x => new Fastnotesapi { Id = x.Id });

            return cambiados.Concat(eliminados);
        }

        public override void Update(Fastnotesapi entity)
        {
            entity.Timestamp = DateTime.Now.ToMexicoTime();
            base.Update(entity);
        }

        public override void Delete(Fastnotesapi entity)
        {
            if(entity.Eliminado == 0)
            {
                entity.Timestamp = DateTime.Now.ToMexicoTime();
                entity.Eliminado = 1;
                base.Update(entity);
            }

            int TTL = 64;

            var fechaEliminar = DateTime.Now.ToMexicoTime().Subtract(TimeSpan.FromDays(TTL));

            var porEliminar = base.GetAll().Where(x=> x.Eliminado == 1 && x.Timestamp < fechaEliminar);

            foreach (var item in porEliminar)
            {
                Context.Remove(item);
            }
            Context.SaveChanges();
        }

        public override bool IsValid(Fastnotesapi entity, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            if(string.IsNullOrWhiteSpace(entity.Titulo) && string.IsNullOrWhiteSpace(entity.Contenido))
            {
                validationErrors.Add("El titulo y el contenido de la nota se encuentra vacia");
            }
            return validationErrors.Count == 0;
        }


    }
}
