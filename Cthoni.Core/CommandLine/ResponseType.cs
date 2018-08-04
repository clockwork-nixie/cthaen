using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public enum ResponseType
    {
        [UsedImplicitly] Unknown = 0,
        Error,
        NotFound,
        Ok,
        Text,
        Trace,
        Quit
    }
}
