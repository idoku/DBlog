using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public class TransQueryResponse : TransResponse
    {
        [JsonProperty("retData")]
        public TransData TransData { get; set; }
    }
}
