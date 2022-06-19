using FastNotesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FastNotesAPI.Repositories
{
    public class VideoGamesRepository : Repository<VideogameT>
    {
        public VideoGamesRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<VideogameT> GetAll()
        {
            return base.GetAll();
        }

        public override void Update(VideogameT entity)
        {
            base.Update(entity);
        }

        public override void Delete(VideogameT entity)
        {
            base.Delete(entity);
        }

    }
}
