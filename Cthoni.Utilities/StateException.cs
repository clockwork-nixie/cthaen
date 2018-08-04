using System;
using JetBrains.Annotations;

namespace Cthoni.Utilities
{
    public class StateException : BaseException
    {
        [UsedImplicitly] public StateException([NotNull] string message) : base(message) { }
        [UsedImplicitly] public StateException([NotNull] string message, Exception inner) : base(message, inner) { }
    }
}
