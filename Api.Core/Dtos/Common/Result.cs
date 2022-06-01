using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    public class Result
    {
        public Result()
        {
            HasErrors = false;
            Messages = new List<string>();
        }

        public bool HasErrors { get; set; }
        public IList<string> Messages { get; set; }
    }
}
