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

        /// <summary>
        /// 将string转换成bool。
        /// <para>当值为"true""TRUE""True"之一时返回true，否则返回false。</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            return value == "true"|| value == "TRUE"||value == "True";
        }

        #region 加密相关

        const int DefaultSeed = 63;

        /// <summary>
        /// 对字符串进行轻量的加密，使用Decrypt方法解密
        /// </summary>
        /// <param name="original"></param>
        /// <param name="Seed">种子</param>
        /// <returns></returns>
        public static string Encipher(this string original,int Seed = 0)
        {
            var array = original.ToCharArray();
            var tempseed = GetSeed(Seed);
            string result = "";
            foreach (var item in array)
            {
                int tempvalue = (int)item;
                result += (tempvalue + tempseed).ToString() + ".";
            }

            return result;
        }

        private static int GetSeed(int seed)
        {
            return seed == 0 ? DefaultSeed : seed;
        }

        /// <summary>
        /// 解密密文
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="Seed">种子</param>
        /// <returns></returns>
        public static string Decrypt(this string ciphertext,int Seed = 0)
        {
            string[] result = ciphertext.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            var tempseed = GetSeed(Seed);

            string original = "";
            foreach (var item in result)
            {
                int tempvalue = item.ToInt();
                char temp = (char)(tempvalue-tempseed);
                original += temp;
            }
            return original;
        }

        #endregion
    }
}
