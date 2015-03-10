using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public abstract class TransResponse
    {
        [JsonProperty("errNum")]
        public int ErrNum { get; set; }

         [JsonProperty("errMsg")]
        public string  ErrMsg { get; set; }

         public string Body { get; set; }

         public string ReqUrl { get; set; }

         public bool IsError
         {
             get
             {
                 return this.ErrNum == 0 && this.ErrMsg=="success";
             }
         }
    }


}
