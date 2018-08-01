using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface ICommandLineProcessor
    {
        [NotNull] CommandLineResponse Process([NotNull] string command);
    }
}
