using FastNotesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FastNotesAPI.Repositories
{
    public class LoginRepository : Repository<LoginModel>
    {
        public LoginRepository(DbContext context) : base(context)
        {
        }

        public override IEnumerable<LoginModel> GetAll()
        {
            return base.GetAll();
        }

    }
}
