namespace Poi
{
    /// <summary>
    /// 可恢复属性
    /// </summary>
    public interface IRestoreProperty
    {
        /// <summary>
        /// 恢复时间间隔
        /// </summary>
        float TimeInterval { get; set; }
        /// <summary>
        /// 距离下次恢复时间
        /// </summary>
        float RestoreCooldownTime { get; set; }
        /// <summary>
        /// 禁用恢复时间
        /// </summary>
        float RestoreDisableTime { get; set; }
        /// <summary>
        /// 每次恢复
        /// </summary>
        double RestorePerTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        void TickRestore(float time);
    }
}