using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    public class ValidationException : Exception
    {
        public Result Result { get; set; }

        public ValidationException(Result result)
            : base("Validation exception")
        {
            Result = result;
        }
    }
}
