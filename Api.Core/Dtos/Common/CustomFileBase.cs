using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Dtos.Common
{
    class CustomFileBase
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        //public System.Web.HttpPostedFileBase File { get; set; } 
    }
}
