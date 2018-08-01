using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface ICommandLineProcessor
    {
        [NotNull] Response Process([NotNull] string command);
    }
}
