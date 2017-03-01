using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 16进制颜色
    /// </summary>
    public struct HexColor
    {
        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public byte a;
        /// <summary>
        /// Blue component of the color.
        /// </summary>
        public byte b;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public byte g;
        /// <summary>
        /// Red component of the color.
        /// </summary>
        public byte r;

        public string hexCode;
        public HexColor(string hex)
        {
            if (hex.Length!= 8)
            {
                throw new ArgumentException("格式不对");
            }
            hexCode = hex;
            string[] code = hex.Split(2);
            r = byte.Parse(code[0], System.Globalization.NumberStyles.AllowHexSpecifier);
            g = byte.Parse(code[1], System.Globalization.NumberStyles.AllowHexSpecifier);
            b = byte.Parse(code[2], System.Globalization.NumberStyles.AllowHexSpecifier);
            a = byte.Parse(code[3], System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        public HexColor(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            hexCode = Convert.ToString(r, 16) + Convert.ToString(g, 16) +
                Convert.ToString(b, 16) + Convert.ToString(a, 16);
        }

        public static HexColor Parse(string hexcode)
        {
            return new HexColor(hexcode);
        }

        public static implicit operator Color32(HexColor c)
        {
            return new Color32(c.r, c.g, c.b, c.a);
        }

        public static implicit operator Color(HexColor c)
        {
            return c;
        }

        public static implicit operator HexColor(Color32 c)
        {
            return new HexColor(c.r,c.g,c.b,c.a);
        }

        public override string ToString()
        {
            return hexCode;
        }
    }
}
