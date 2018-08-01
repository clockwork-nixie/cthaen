using System;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandLineException : Exception
    {
        public CommandLineException([NotNull] string message)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
        }
    }
}
