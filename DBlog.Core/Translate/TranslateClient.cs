using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public class TranslateClient:ITranslateClient
    {
        private const string SERVER_URL = "http://apistore.baidu.com/microservice/";

        private WebUtils webUtils = new WebUtils();

        public TranslateClient() { }

        public T Execute<T>(ITransRequest<T> request) where T : TransResponse
        {
            string url = SERVER_URL + request.GetApiName();
            var txtParms = new TransDictionary(request.GetParameters());
            string body = webUtils.DoGet(url, txtParms);
            T rsp = TransUtils.ParseResponse<T>(body);
            rsp.Body = body;
            rsp.ReqUrl = url;
            return rsp;
        }
         

        #region properties
        /// <summary>
        /// translate content
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// source language
        /// </summary>
        public string Form { get; set; }

        /// <summary>
        /// Destination language
        /// </summary>
        public string To { get; set; }

        #endregion

      
    }
}
