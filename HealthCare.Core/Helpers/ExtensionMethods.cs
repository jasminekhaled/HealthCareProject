using HealthCare.Core.Models.AuthModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Helpers
{
    public static class ExtensionMethods
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void InitializeHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public static int FindFirst(this HttpContext httpContext)
        { 
            string userId = httpContext.User.FindFirst("userId").Value;
            return int.Parse(userId);
        }
    }
}
