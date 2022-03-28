using System.Runtime.CompilerServices;

namespace TinyWordle
{
    // https://github.com/MichalStrehovsky/SeeSharpSnake/blob/master/Game/Random.cs
    struct Random
    {
        public uint _val;

        public Random(uint seed)
        {
            _val = seed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Next() => _val = (1103515245 * _val + 12345) % 2147483648;
    }
}
