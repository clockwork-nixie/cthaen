using System;
using JetBrains.Annotations;

namespace Cthoni.Utilities
{
    public abstract class BaseException : Exception
    {
        // ReSharper disable once IntroduceOptionalParameters.Global
        protected BaseException([NotNull] string message)
            : this(message, null)
        {
            // EMPTY
        }


        protected BaseException([NotNull] string message, Exception inner)
            : base(message, inner)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
        }
    }
}
