using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public class TransQueryRequest : ITransRequest<TransQueryResponse>
    {
        public string Query { get; set; }

        public string Form { get; set; }

        public string To { get; set; }

        public string GetApiName()
        {
            return "translate";
        }

        public IDictionary<string, string> GetParameters()
        {
            TransDictionary parameters = new TransDictionary();
            parameters.Add("query", this.Query);
            parameters.Add("from", this.Form);
            parameters.Add("to", this.To);
            return parameters;
        }
    }
}
