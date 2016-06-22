using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.UI
{
    /// <summary>
    /// UI扩展
    /// </summary>
    public static class PoiExtention
    {
        #region Dropdown

        /// <summary>
        /// 设置下拉选项
        /// </summary>
        /// <typeparam name="T">一个枚举类型</typeparam>
        /// <param name="dp"></param>
        /// <exception cref="ArgumentException">参数不是枚举</exception>
        public static void SetOptions<T>(this Dropdown dp)
        {
            var ops = Enum.GetNames(typeof(T));
            dp.SetOptions(ops);
        }

        /// <summary>
        /// 设置下拉选项
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="strText"></param>
        public static void SetOptions(this Dropdown dp, IList<string> strText)
        {
            dp.options = StringListToOptionDataList(strText);
        }

        static List<Dropdown.OptionData> StringListToOptionDataList(IList<string> strText)
        {
            List<Dropdown.OptionData> result = new List<Dropdown.OptionData>();

            foreach (var value in strText)
            {
                result.Add(StringToOptionData(value));
            }

            return result;
        }

        static Dropdown.OptionData StringToOptionData(string strText)
        {
            return new Dropdown.OptionData(strText);
        }

        public static Dropdown.OptionData GetCurrent(this Dropdown dp)
        {
            return dp.options[dp.value];
        }

        #endregion
    }
}
