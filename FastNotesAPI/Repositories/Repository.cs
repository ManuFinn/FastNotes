using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FastNotesAPI.Repositories
{
    public class Repository<T> where T : class
    {
        public DbContext Context { get; set; }

        public Repository(DbContext context)
        {
            Context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual T Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }

        public virtual bool IsValid(T entity, out List<string> validationErrors)
        {
            validationErrors = new List<string>();
            return true;
        }
    }
}
