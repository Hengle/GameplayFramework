using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 纹理加载
    /// </summary>
    public class TextureUtil
    {
        /// <summary>
        /// 根据路径加载一个纹理
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Texture2D LoadTexture(string path)
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(LoadBin(path));
            return texture;
        }


        static byte[] LoadBin(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            return buf;
        }
    }
}
