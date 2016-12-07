namespace Poi
{
    /// <summary>
    /// 区间值
    /// </summary>
    /// <typeparam name="M">区间类型</typeparam>
    /// <typeparam name="C">当前值类型</typeparam>
    public interface IRangeProperty<M,C>:IMaxLimit<M,C>
        where M:struct where C:struct
    {
        /// <summary>
        /// 许可的最小值
        /// </summary>
        M Min { get; set; }
    }


    public interface IMaxLimit<M,C> where M : struct where C : struct
    {
        /// <summary>
        /// 许可的最大值
        /// </summary>
        M Max { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        C Current { get; set; }
    }
}