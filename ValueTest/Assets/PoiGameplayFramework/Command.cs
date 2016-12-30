namespace Poi
{
    /// <summary>
    /// 输入命令
    /// </summary>
    public class InputCommand
    {
        public float Horizontal { get;  set; }
        public bool Jump { get;  set; }
        public float MouseX { get; internal set; }
        public float MouseY { get; internal set; }
        public float Vertical { get;  set; }

        public static implicit operator bool(InputCommand c)
        {
            return c != null;
        }
    }
}