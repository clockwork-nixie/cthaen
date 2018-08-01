using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public enum CommandLineResponseType
    {
        [UsedImplicitly] Unknown = 0,
        Error,
        Text,
        Quit
    }
}
