using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TinyWordle
{
    public static class TinyConsole
    {
        [DllImport("kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
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
            IntPtr consoleHandle = GetStdHandle(-11);
            uint consoleMode;

            // Get the current console mode
            GetConsoleMode(consoleHandle, out consoleMode);

            // Modify the console mode to enable virtual terminal processing
            consoleMode |= 0x0004;
            SetConsoleMode(consoleHandle, consoleMode);
        }

        public static void Write(string value)
        {
            IntPtr consoleHandle = GetStdHandle(-11);
            uint charsWritten;

            WriteConsole(consoleHandle, value, (uint)value.Length, out charsWritten, IntPtr.Zero);
        }

        public static void Clear()
        {
            IntPtr hConsole = GetStdHandle(-11);
            CONSOLE_SCREEN_BUFFER_INFO csbi;
            GetConsoleScreenBufferInfo(hConsole, out csbi);

            COORD dwTopLeft = new COORD();
           
            FillConsoleOutputCharacter(hConsole, ' ', (uint)(csbi.dwSize.X * csbi.dwSize.Y), dwTopLeft, out _);

            SetConsoleCursorPosition(hConsole, dwTopLeft);
        }

        public static string ReadLine()
        {
            IntPtr hConsoleInput = GetStdHandle(-10);
            StringBuilder buffer = new StringBuilder(512*8);
            uint charsRead;

            ReadConsole(hConsoleInput, buffer, (uint)buffer.Capacity, out charsRead, IntPtr.Zero);

            string input = buffer.ToString().TrimEnd();

            return input;
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