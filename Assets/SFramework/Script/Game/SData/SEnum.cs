public class SEnum
{
    public static string ExportTag = "$EXPORT$"; 
    public enum SafeModes
    {
        Nope,
        EditorOnly,
        RuntimeOnly,
        Full
    }

    public enum MouseClick
    {
        Left,
        Right,
        Middle
    }

    public enum Performance
    {
        Low = 0,
        Middle = 1,
        High = 2,
    }
}
