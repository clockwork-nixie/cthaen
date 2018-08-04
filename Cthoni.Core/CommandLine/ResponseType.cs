using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public enum ResponseType
    {
        [UsedImplicitly] Unknown = 0,
        NotFound,
        Ok,
        Text,
        Trace,
        Load,
        Quit,
    }
}
