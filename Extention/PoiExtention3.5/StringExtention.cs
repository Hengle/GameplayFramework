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

        /// <summary>
        /// 将一个字符串转换成枚举
        /// </summary>
        /// <typeparam name="T">提供一个枚举类型</typeparam>
        /// <param name="value"></param>
        /// <returns>返回对应的枚举值</returns>
        /// <exception cref="ArgumentException">所给泛型不是枚举</exception>
        public static T ToEnum<T>(this string value)
            where T:struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("所给泛型不是枚举");

            return  (T)Enum.Parse(typeof(T), value);
        }
    }
}
