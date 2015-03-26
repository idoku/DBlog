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

        public static string Translate(string query,Language from,Language to)
        {
            var req = new TransQueryRequest();
            req.Query = query;
            req.From = from.ToString();
            req.To = to.ToString();
            TransQueryResponse res = TransClient.Execute(req);
            if(res.ErrMsg=="success")
            {
                return res.TransData.Result[0].DstTest;
            }
            return "";
        }
    }

    public enum Language
    {
        /// <summary>
        /// 中文
        /// </summary>
        zh,
        /// <summary>
        /// 英文
        /// </summary>
        en,
        /// <summary>
        /// 日文
        /// </summary>
        jp,
        /// <summary>
        /// 韩文
        /// </summary>
        kor,
        /// <summary>
        /// 西班牙语
        /// </summary>
        spa,
        /// <summary>
        /// 法语
        /// </summary>
        fra,
        /// <summary>
        /// 泰语
        /// </summary>
        th,
        /// <summary>
        /// 阿拉伯语
        /// </summary>
        ara,
        /// <summary>
        /// 俄语
        /// </summary>
        ru,
        /// <summary>
        /// 葡萄牙语
        /// </summary>
        pt,
        /// <summary>
        /// 粤语
        /// </summary>
        yue,
        /// <summary>
        /// 文言文
        /// </summary>
        wyw,
        /// <summary>
        /// 自动
        /// </summary>
        auto,
        /// <summary>
        /// 德语
        /// </summary>
        de,
        /// <summary>
        /// 意大利
        /// </summary>
        it
    }
}
