using Cthoni.Core.Context;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface ICommandLineProcessor
    {
        [NotNull] IContext Context { get; }
        [NotNull] Response Process([NotNull] string command);
    }
}
