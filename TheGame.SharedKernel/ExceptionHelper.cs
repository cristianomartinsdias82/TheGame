using System;
using System.Threading;

namespace TheGame.SharedKernel
{
    public static class ExceptionHelper
    {
        public static ArgumentNullException ArgNullEx(string argumentName)
            => throw new ArgumentNullException($"{argumentName} argument cannot be null.");
    }
}
