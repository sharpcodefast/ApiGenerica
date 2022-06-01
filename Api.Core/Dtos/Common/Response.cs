using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    [Serializable]
    public class Response<T>
    {
        public Response()
        {
            Result = new Result();
        }

        public Result Result { get; set; }
        public T Data { get; set; }
    }
}
