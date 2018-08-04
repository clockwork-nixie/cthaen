using Cthoni.Core.Science;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public interface IContext
    {
        [NotNull] IFactBase Facts { get; }
    }
}