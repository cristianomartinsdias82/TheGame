using System;

namespace TheGame.SharedKernel.Helpers
{
    public static class ExceptionHelper
    {
        public static ArgumentNullException ArgNullEx(string argumentName)
            => throw new ArgumentNullException($"{argumentName} argument cannot be null.");
    }
}
