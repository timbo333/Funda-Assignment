using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions.Options
{
    public class FundaApiOptions
    {
        public const string FundaApi = "FundaApi";
        public string BaseUrl { get; set; }
        public string AccessToken { get; set; }
        public string ResponseContentType { get; set; }
    }
}
