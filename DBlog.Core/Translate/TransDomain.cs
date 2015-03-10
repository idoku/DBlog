using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    [Serializable]
    public class TransData
    {
         [JsonProperty("from")]
        public string Form { get; set; }

         [JsonProperty("to")]
        public string To { get; set; }

         [JsonProperty("trans_result")]
        public TransResult[] Result { get; set; }
    }

    [Serializable]
    public class TransResult
    {
         [JsonProperty("src")]
        public string SrcText { get; set; }

         [JsonProperty("dst")]
        public string DstTest { get; set; }
    }
}
