using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public static class TransUtils
    {
        private static IDictionary<string, string> SplitUrlQuery(string query)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            string[] pairs = query.Split(new char[] { '&' });
            if (pairs != null && pairs.Length > 0)
            {
                foreach (string pair in pairs)
                {
                    string[] oneParam = pair.Split(new char[] { '=' }, 2);
                    if (oneParam != null && oneParam.Length == 2)
                    {
                        result.Add(oneParam[0], oneParam[1]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 清除字典中值为空的项。
        /// </summary>
        /// <param name="dict">待清除的字典</param>
        /// <returns>清除后的字典</returns>
        public static IDictionary<string, T> CleanupDictionary<T>(IDictionary<string, T> dict)
        {
            IDictionary<string, T> newDict = new Dictionary<string, T>(dict.Count);
            IEnumerator<KeyValuePair<string, T>> dem = dict.GetEnumerator();

            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                T value = dem.Current.Value;
                if (value != null)
                {
                    newDict.Add(name, value);
                }
            }

            return newDict;
        }


 
 

        /// <summary>
        /// 根据API名称获取响应根节点名称。
        /// </summary>
        /// <param name="api">API名称</param>
        /// <returns></returns>
        public static string GetRootElement(string api)
        {
            int pos = api.IndexOf(".");
            if (pos != -1 && api.Length > pos)
            {
                api = api.Substring(pos + 1).Replace('.', '_');
            }
            return api + "_response";
        }

 

        /// <summary>
        /// 把JSON解释为API响应对象。
        /// </summary>
        /// <typeparam name="T">API响应类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>API响应对象</returns>
        public static T ParseResponse<T>(string json) where T : TransResponse
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取从1970年1月1日到现在的毫秒总数。
        /// </summary>
        /// <returns>毫秒数</returns>
        public static long GetCurrentTimeMillis(this DateTime time)
        {
            return (long)time.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static string CreateErrorJson(int errCode, string errMsg)
        {
            IDictionary<string, object> errObj = new Dictionary<string, object>();
            errObj.Add("request_id", 0);
            errObj.Add("error_code", errCode);
            errObj.Add("error_msg", errMsg);

            return JsonConvert.SerializeObject(errObj);
        }
    }
}
