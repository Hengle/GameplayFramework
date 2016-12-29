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

    /// <summary>
    /// 对Pawn的命令
    /// </summary>
    public class Command
    {
        public float? Angle { get; set; }
        public PawnState MoveState { get; internal set; }

        public static implicit operator bool(Command c)
        {
            return c != null;
        }
    }
}