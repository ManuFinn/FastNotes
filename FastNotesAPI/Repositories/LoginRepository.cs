using FastNotesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FastNotesAPI.Repositories
{
    public class LoginRepository : Repository<UsersT>
    {
        public LoginRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<UsersT> GetAll()
        {
            return base.GetAll();
        }

    }
}
