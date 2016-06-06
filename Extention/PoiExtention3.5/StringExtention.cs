using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// string扩展
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// 使用int.Parse转换一个字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
    }
}
