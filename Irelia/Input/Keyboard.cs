using System;
using SlimDX.DirectInput;
using DIKeyboard = SlimDX.DirectInput.Keyboard;

namespace Input
{
    public enum Key
    {
        // Summary:
        //     The number 0.
        D0 = 0,
        //
        // Summary:
        //     The number 1.
        D1 = 1,
        //
        // Summary:
        //     The number 2.
        D2 = 2,
        //
        // Summary:
        //     The number 3.
        D3 = 3,
        //
        // Summary:
        //     The number 4.
        D4 = 4,
        //
        // Summary:
        //     The number 5.
        D5 = 5,
        //
        // Summary:
        //     The number 6.
        D6 = 6,
        //
        // Summary:
        //     The number 7.
        D7 = 7,
        //
        // Summary:
        //     The number 8.
        D8 = 8,
        //
        // Summary:
        //     The number 9.
        D9 = 9,
        //
        // Summary:
        //     The letter A.
        A = 10,
        //
        // Summary:
        //     The letter B.
        B = 11,
        //
        // Summary:
        //     The letter C.
        C = 12,
        //
        // Summary:
        //     The letter D.
        D = 13,
        //
        // Summary:
        //     The letter E.
        E = 14,
        //
        // Summary:
        //     The letter F.
        F = 15,
        //
        // Summary:
        //     The letter G.
        G = 16,
        //
        // Summary:
        //     The letter H.
        H = 17,
        //
        // Summary:
        //     The letter I.
        I = 18,
        //
        // Summary:
        //     The letter J.
        J = 19,
        //
        // Summary:
        //     The letter K.
        K = 20,
        //
        // Summary:
        //     The letter L.
        L = 21,
        //
        // Summary:
        //     The letter M.
        M = 22,
        //
        // Summary:
        //     The letter N.
        N = 23,
        //
        // Summary:
        //     The letter O.
        O = 24,
        //
        // Summary:
        //     The letter P.
        P = 25,
        //
        // Summary:
        //     The letter Q.
        Q = 26,
        //
        // Summary:
        //     The letter R.
        R = 27,
        //
        // Summary:
        //     The letter S.
        S = 28,
        //
        // Summary:
        //     The letter T.
        T = 29,
        //
        // Summary:
        //     The letter U.
        U = 30,
        //
        // Summary:
        //     The letter V.
        V = 31,
        //
        // Summary:
        //     The letter W.
        W = 32,
        //
        // Summary:
        //     The letter X.
        X = 33,
        //
        // Summary:
        //     The letter Y.
        Y = 34,
        //
        // Summary:
        //     The letter Z.
        Z = 35,
        //
        // Summary:
        //     The AbntC1 key on Brazillian keyboards.
        AbntC1 = 36,
        //
        // Summary:
        //     The AbntC2 key on Brazillian keyboards.
        AbntC2 = 37,
        //
        // Summary:
        //     The apostrophe key.
        Apostrophe = 38,
        //
        // Summary:
        //     The Applications key.
        Applications = 39,
        //
        // Summary:
        //     The Japanese At key.
        AT = 40,
        //
        // Summary:
        //     The Japanese Ax key.
        AX = 41,
        //
        // Summary:
        //     The Backspace.
        Backspace = 42,
        //
        // Summary:
        //     The back slash key.
        Backslash = 43,
        //
        // Summary:
        //     The calculator key.
        Calculator = 44,
        //
        // Summary:
        //     The Caps Lock key.
        CapsLock = 45,
        //
        // Summary:
        //     The colon key.
        Colon = 46,
        //
        // Summary:
        //     The comma key.
        Comma = 47,
        //
        // Summary:
        //     The Japanese Convert key.
        Convert = 48,
        //
        // Summary:
        //     The Delete key.
        Delete = 49,
        //
        // Summary:
        //     The Down Arrow key.
        DownArrow = 50,
        //
        // Summary:
        //     The End key.
        End = 51,
        //
        // Summary:
        //     The equals key.
        Equals = 52,
        //
        // Summary:
        //     The Escape key.
        Escape = 53,
        //
        // Summary:
        //     The F1 key.
        F1 = 54,
        //
        // Summary:
        //     The F2 key.
        F2 = 55,
        //
        // Summary:
        //     The F3 key.
        F3 = 56,
        //
        // Summary:
        //     The F4 key.
        F4 = 57,
        //
        // Summary:
        //     The F5 key.
        F5 = 58,
        //
        // Summary:
        //     The F6 key.
        F6 = 59,
        //
        // Summary:
        //     The F7 key.
        F7 = 60,
        //
        // Summary:
        //     The F8 key.
        F8 = 61,
        //
        // Summary:
        //     The F9 key.
        F9 = 62,
        //
        // Summary:
        //     The F10 key.
        F10 = 63,
        //
        // Summary:
        //     The F11 key.
        F11 = 64,
        //
        // Summary:
        //     The F12 key.
        F12 = 65,
        //
        // Summary:
        //     The F13 key.
        F13 = 66,
        //
        // Summary:
        //     The F14 key.
        F14 = 67,
        //
        // Summary:
        //     The F15 key.
        F15 = 68,
        //
        // Summary:
        //     The grav accent (`) key.
        Grave = 69,
        //
        // Summary:
        //     The Home key.
        Home = 70,
        //
        // Summary:
        //     The Insert key.
        Insert = 71,
        //
        // Summary:
        //     The Japanese Kana key.
        Kana = 72,
        //
        // Summary:
        //     The Japanese Kanji key.
        Kanji = 73,
        //
        // Summary:
        //     The left square bracket key.
        LeftBracket = 74,
        //
        // Summary:
        //     The left Ctrl key.
        LeftControl = 75,
        //
        // Summary:
        //     The Left Arrow key.
        LeftArrow = 76,
        //
        // Summary:
        //     The left Alt key.
        LeftAlt = 77,
        //
        // Summary:
        //     The left Shift key.
        LeftShift = 78,
        //
        // Summary:
        //     The left Windows key.
        LeftWindowsKey = 79,
        //
        // Summary:
        //     The Mail key.
        Mail = 80,
        //
        // Summary:
        //     The Media Select key.
        MediaSelect = 81,
        //
        // Summary:
        //     The Media Stop key.
        MediaStop = 82,
        //
        // Summary:
        //     The minus key.
        Minus = 83,
        //
        // Summary:
        //     The Mute key.
        Mute = 84,
        //
        // Summary:
        //     The My Computer key.
        MyComputer = 85,
        //
        // Summary:
        //     The Next Track key.
        NextTrack = 86,
        //
        // Summary:
        //     The Japanese No Convert key.
        NoConvert = 87,
        //
        // Summary:
        //     The NumberLock key.
        NumberLock = 88,
        //
        // Summary:
        //     The number 0 on the NumberPad.
        NumberPad0 = 89,
        //
        // Summary:
        //     The number 1 on the NumberPad.
        NumberPad1 = 90,
        //
        // Summary:
        //     The number 2 on the NumberPad.
        NumberPad2 = 91,
        //
        // Summary:
        //     The number 3 on the NumberPad.
        NumberPad3 = 92,
        //
        // Summary:
        //     The number 4 on the NumberPad.
        NumberPad4 = 93,
        //
        // Summary:
        //     The number 5 on the NumberPad.
        NumberPad5 = 94,
        //
        // Summary:
        //     The number 6 on the NumberPad.
        NumberPad6 = 95,
        //
        // Summary:
        //     The number 7 on the NumberPad.
        NumberPad7 = 96,
        //
        // Summary:
        //     The number 8 on the NumberPad.
        NumberPad8 = 97,
        //
        // Summary:
        //     The number 9 on the NumberPad.
        NumberPad9 = 98,
        //
        // Summary:
        //     The comma key on the NumberPad.
        NumberPadComma = 99,
        //
        // Summary:
        //     The Enter key on the NumberPad.
        NumberPadEnter = 100,
        //
        // Summary:
        //     The equals key on the NumberPad.
        NumberPadEquals = 101,
        //
        // Summary:
        //     The minus key on the NumberPad.
        NumberPadMinus = 102,
        //
        // Summary:
        //     The period key on the NumberPad.
        NumberPadPeriod = 103,
        //
        // Summary:
        //     The plus key on the NumberPad.
        NumberPadPlus = 104,
        //
        // Summary:
        //     The slash key on the NumberPad.
        NumberPadSlash = 105,
        //
        // Summary:
        //     The asterisk key on the NumberPad.
        NumberPadStar = 106,
        //
        // Summary:
        //     The British and German OEM102 key.
        Oem102 = 107,
        //
        // Summary:
        //     The Page Down key.
        PageDown = 108,
        //
        // Summary:
        //     The Page Up key.
        PageUp = 109,
        //
        // Summary:
        //     The Pause key.
        Pause = 110,
        //
        // Summary:
        //     The period key.
        Period = 111,
        //
        // Summary:
        //     The Play/Pause key.
        PlayPause = 112,
        //
        // Summary:
        //     The Power key.
        Power = 113,
        //
        // Summary:
        //     The Previous Track key.
        PreviousTrack = 114,
        //
        // Summary:
        //     The right square bracket key.
        RightBracket = 115,
        //
        // Summary:
        //     The right Ctrl key.
        RightControl = 116,
        //
        // Summary:
        //     The Return/Enter key.
        Return = 117,
        //
        // Summary:
        //     The Right Arrow key.
        RightArrow = 118,
        //
        // Summary:
        //     The right Alt key.
        RightAlt = 119,
        //
        // Summary:
        //     The right Shift key.
        RightShift = 120,
        //
        // Summary:
        //     The right Windows key.
        RightWindowsKey = 121,
        //
        // Summary:
        //     The Scroll Lock key.
        ScrollLock = 122,
        //
        // Summary:
        //     The semicolon key.
        Semicolon = 123,
        //
        // Summary:
        //     The slash key.
        Slash = 124,
        //
        // Summary:
        //     The Sleep key.
        Sleep = 125,
        //
        // Summary:
        //     The Spacebar.
        Space = 126,
        //
        // Summary:
        //     The Stop key.
        Stop = 127,
        //
        // Summary:
        //     The Print Screen key.
        PrintScreen = 128,
        //
        // Summary:
        //     The Tab key.
        Tab = 129,
        //
        // Summary:
        //     The Japanese Underline key.
        Underline = 130,
        //
        // Summary:
        //     An unlabeled key.
        Unlabeled = 131,
        //
        // Summary:
        //     The Up Arrow key.
        UpArrow = 132,
        //
        // Summary:
        //     The Volume Down key.
        VolumeDown = 133,
        //
        // Summary:
        //     The Volume Up key.
        VolumeUp = 134,
        //
        // Summary:
        //     The Wake key.
        Wake = 135,
        //
        // Summary:
        //     The Web Backwards key.
        WebBack = 136,
        //
        // Summary:
        //     The Web Favorites key.
        WebFavorites = 137,
        //
        // Summary:
        //     The Web Forwards key.
        WebForward = 138,
        //
        // Summary:
        //     The Web Home key.
        WebHome = 139,
        //
        // Summary:
        //     The Web Refresh key.
        WebRefresh = 140,
        //
        // Summary:
        //     The Web Search key.
        WebSearch = 141,
        //
        // Summary:
        //     The Web Stop key.
        WebStop = 142,
        //
        // Summary:
        //     The Japanese Yen key.
        Yen = 143,
        Unknown = 144,
    }

    public sealed class Keyboard
    {
        private readonly DIKeyboard keyboard;

        public Keyboard(IntPtr windowHandle)
        {
            var dinput = new DirectInput();

            this.keyboard = new DIKeyboard(dinput);
            this.keyboard.SetCooperativeLevel(windowHandle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);
            this.keyboard.Acquire();
        }

        public bool IsKeyDown(Key key)
        {
            if (this.keyboard.Acquire().IsFailure)
                return false;

            if (this.keyboard.Poll().IsFailure)
                return false;

            var state = new KeyboardState();
            if (this.keyboard.GetCurrentState(ref state).IsFailure)
                return false;

            return state.IsPressed(key.ToDIKey());
        }

        //public void Release()
        //{
        //    if (this.keyboard != null)
        //    {
        //        this.keyboard.Unacquire();
        //        this.keyboard.Dispose();
        //    }
        //}
    }
}
