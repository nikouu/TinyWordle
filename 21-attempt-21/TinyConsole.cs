using System.Runtime.InteropServices;
using System.Text;

namespace TinyWordle
{
    // https://stackoverflow.com/a/75958239
    public static class TinyConsole
    {
        [DllImport("msvcr120.dll")]
        public static extern int printf(string format);
    }
}
