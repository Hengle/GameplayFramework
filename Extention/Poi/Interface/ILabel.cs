using System;

namespace Poi
{
    /// <summary>
    /// 标签，含有ID和Name
    /// </summary>
    public interface ILabel:IID,IName
    {
    }

    /// <summary>
    /// 标签
    /// </summary>
    public class Label : ILabel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get
            ;

            set
            ;
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get
            ;

            set
            ;
        }
    }
}
