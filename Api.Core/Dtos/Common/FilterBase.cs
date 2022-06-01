using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    public class FilterBase
    {
        public string MultiColumnSearchText { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
    }
}
