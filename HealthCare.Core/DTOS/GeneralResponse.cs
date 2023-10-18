using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS
{
    public class GeneralResponse<Type> where Type : class
    {
        public Type Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }

    }
}
