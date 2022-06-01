using System.IO;

namespace Api.Core.Infrastructure
{
    public class CustomFileBase
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        //public System.Web.HttpPostedFileBase File { get; set; }
    }
}
