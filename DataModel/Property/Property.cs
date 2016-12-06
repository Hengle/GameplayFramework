namespace Poi
{
    public class Property
    {
        /// <summary>
        /// 属性刷新类型
        /// </summary>
        public virtual ValueChangedType ChangedType { get; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public virtual PropertyType PropertyType { get; protected set; }
    }
}