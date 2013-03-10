using DIKey = SlimDX.DirectInput.Key;

namespace Input
{
    public static class DIMapping
    {
        public static Key ToKey(this DIKey diKey)
        {
            return (Key)diKey;
        }

        public static DIKey ToDIKey(this Key key)
        {
            return (DIKey)key;
        }

        public static MouseButton ToMouseButton(this Mouse.DIButton diButton)
        {
            return (MouseButton)diButton;
        }
    }
}
