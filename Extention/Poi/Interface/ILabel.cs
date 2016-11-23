using System;

namespace Poi
{
    /// <summary>
    /// 标签，含有ID和Name
    /// </summary>
    public interface ILabel:IID,IName
    {
    }

    public class Label : ILabel
    {
        public int ID
        {
            get
            ;

            set
            ;
        }

        public string Name
        {
            get
            ;

            set
            ;
        }
    }
}
