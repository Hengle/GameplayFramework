namespace Poi
{
    public abstract class BaseProperty : Property, IRangeProperty<int,double>
    {
        /// <summary>
        /// 许可的最大值
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 许可的最小值
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public double Current { get; set; }
    }
}