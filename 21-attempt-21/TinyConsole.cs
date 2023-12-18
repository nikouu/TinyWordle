using System.Runtime.InteropServices;
using System.Text;

namespace TinyWordle
{
    // https://stackoverflow.com/a/75958239
    // A rough look at using p/invoke to replace Console
    public static class TinyConsole
    {
        [DllImport("msvcr120.dll")]
        public static extern int printf(string format);

        [DllImport("msvcr120.dll")]
        public static extern int system(string command);

        [DllImport("msvcr120.dll")]
        private static extern IntPtr gets(StringBuilder value);

        public static string ReadLine()
        {
            var value = new StringBuilder();

            gets(value);

            return value.ToString();
        }
    }
}
