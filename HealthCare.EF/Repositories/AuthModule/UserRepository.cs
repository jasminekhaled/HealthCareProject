using HealthCare.Core.IRepositories.AuthModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.AuthRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
