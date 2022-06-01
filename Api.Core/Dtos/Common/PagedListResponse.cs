using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    public class PagedListResponse<T> : Response<IList<T>>
    {
        public int Count { get; set; }
    }
}
