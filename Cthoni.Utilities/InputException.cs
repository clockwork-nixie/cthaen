using System;
using JetBrains.Annotations;

namespace Cthoni.Utilities
{
    public class InputException : BaseException
    {
        [UsedImplicitly] public InputException([NotNull] string message) : base(message) { }
        [UsedImplicitly] public InputException([NotNull] string message, Exception inner) : base(message, inner) { }
    }
}
