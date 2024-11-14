using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace TinyWordle
{
    public static class TinyConsole
    {
        [DllImport("kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32")]
        static extern bool WriteConsole(IntPtr hConsoleOutput, string lpBuffer, uint nNumberOfCharsToWrite, out uint lpNumberOfCharsWritten, IntPtr lpReserved);

        [DllImport("kernel32")]
        static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32")]
        static extern bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, char cCharacter, uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

        [DllImport("kernel32")]
        static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD dwCursorPosition);

        [DllImport("kernel32")]
        static extern bool ReadConsole(IntPtr hConsoleInput, StringBuilder lpBuffer, uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead, IntPtr lpReserved);

        static TinyConsole()
        {
            var consoleHandle = GetStdHandle(-11);

            // Get the current console mode
            GetConsoleMode(consoleHandle, out uint consoleMode);

            // Modify the console mode to enable virtual terminal processing
            consoleMode |= 0x0004;
            SetConsoleMode(consoleHandle, consoleMode);
        }

        public static void Write(string value)
        {
            var consoleHandle = GetStdHandle(-11);

            WriteConsole(consoleHandle, value, (uint)value.Length, out uint charsWritten, IntPtr.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear()
        {
            var hConsole = GetStdHandle(-11);
            GetConsoleScreenBufferInfo(hConsole, out CONSOLE_SCREEN_BUFFER_INFO csbi);

            var dwTopLeft = new COORD();
           
            FillConsoleOutputCharacter(hConsole, ' ', (uint)(csbi.dwSize.X * csbi.dwSize.Y), dwTopLeft, out _);

            SetConsoleCursorPosition(hConsole, dwTopLeft);
        }

        public static string ReadLine()
        {
            var hConsoleInput = GetStdHandle(-10);
            var buffer = new StringBuilder(1024);

            ReadConsole(hConsoleInput, buffer, (uint)buffer.Capacity, out uint charsRead, IntPtr.Zero);

            var input = buffer.ToString();

            return input.Length <= 5 ? input : input[..5];
        }

        struct COORD
        {
            public short X;
            public short Y;
        }

        struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}