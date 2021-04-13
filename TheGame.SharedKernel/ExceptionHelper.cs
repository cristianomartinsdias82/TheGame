using System;

namespace TheGame.SharedKernel
{
    public static class ExceptionHelper
    {
        public static ArgumentNullException ArgNullEx(string message)
            => throw new ArgumentNullException($"{message} argument cannot be null.");
    }
}
