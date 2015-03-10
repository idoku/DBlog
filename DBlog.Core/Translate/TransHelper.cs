using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public class TransHelper
    {
        private static readonly ITranslateClient TransClient = new TranslateClient();

        public static void Translate(string query)
        {
            var req = new TransQueryRequest();
            req.Query = query;
            req.Form = "zh";
            req.To = "en";
            TransQueryResponse res = TransClient.Execute(req);
        }
    }
}
