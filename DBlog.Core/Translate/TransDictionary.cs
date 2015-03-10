using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    /// <summary>
    /// 符合PUSH习惯的纯字符串字典结构。
    /// </summary>
    public class TransDictionary : Dictionary<string, string>
    {
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public TransDictionary() { }

        public TransDictionary(IDictionary<string, string> dictionary)
            : base(dictionary)
        { }

        /// <summary>
        /// 添加一个新的键值对。空键或者空值的键值对将会被忽略。
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">键对应的值，目前支持：string, uint, ulong, double, bool, DateTime类型</param>
        public void Add(string key, object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is string)
            {
                strValue = (string)value;
            }
            else if (value is DateTime?)
            {
                DateTime? dateTime = value as DateTime?;
                strValue = dateTime.Value.ToString(DATE_TIME_FORMAT);
            }
            else if (value is uint?)
            {
                strValue = (value as uint?).Value.ToString();
            }
            else if (value is ulong?)
            {
                strValue = (value as ulong?).Value.ToString();
            }
            else if (value is double?)
            {
                strValue = (value as double?).Value.ToString();
            }
            else if (value is bool?)
            {
                strValue = (value as bool?).Value.ToString().ToLower();
            }
            else
            {
                strValue = value.ToString();
            }

            this.Add(key, strValue);
        }

        public new void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                base.Add(key, value);
            }
        }
    }
}
