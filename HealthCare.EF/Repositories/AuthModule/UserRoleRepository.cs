﻿using HealthCare.Core.IRepositories.AuthModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.AuthRepositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {

        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
