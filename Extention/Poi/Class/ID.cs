namespace Poi
{
    /// <summary>
    /// 线程安全的全局ID
    /// </summary>
    public static class ID
    {
        static int id = 1;
        static string isLocked = "";
        /// <summary>
        /// 取得一个全局的ID
        /// </summary>
        /// <returns></returns>
        static public int GetGlobalID()
        {
            lock (isLocked)
            {
                return id++;
            }
        }
    }
}
