﻿using HealthCare.Core.IRepositories;
using HealthCare.Core.IRepositories.AuthModule;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AuthModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories
{
    public class UploadedFileRepository : BaseRepository<UploadedFile>, IUploadedFileRepository
    {

        public UploadedFileRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
