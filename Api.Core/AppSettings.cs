using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core
{
    public class AppSettings
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public Dictionary<string, string> SecurityApis { get; set; }
        public string Cors { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string Secret { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }

        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
