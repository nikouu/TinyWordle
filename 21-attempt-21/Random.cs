using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TinyWordle
{
    // https://github.com/MichalStrehovsky/SeeSharpSnake/blob/master/Game/Random.cs
    static class Random
    {
        [DllImport("Kernel32")]
        private static extern uint GetTickCount();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Next() => (1103515245 * GetTickCount() + 12345) % 2147483648;
    }
}
