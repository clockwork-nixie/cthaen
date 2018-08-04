using Cthoni.Core.CommandLine;
using Cthoni.Core.Science;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public interface IContext
    {
        [NotNull] IDirectiveSet<Response> Directives { get; }
        [NotNull] IFactBase Facts { get; }
    }
}