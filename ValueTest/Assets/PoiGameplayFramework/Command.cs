namespace Poi
{
    public class Command
    {
        public float Horizontal { get;  set; }
        public bool Jump { get;  set; }
        public float MouseX { get; internal set; }
        public float MouseY { get; internal set; }
        public float Vertical { get;  set; }

        public static implicit operator bool(Command c)
        {
            return c != null;
        }
    }
}