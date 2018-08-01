using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface ICommandLineProcessor
    {
        [NotNull] string Process([NotNull] string command);
    }
}
