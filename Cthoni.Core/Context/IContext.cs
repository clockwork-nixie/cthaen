using Cthoni.Core.Science;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public interface IContext
    {
        [NotNull] ITopic CreateTopic(string name);
        [NotNull] ITopic CurrentTopic { get; }
        void SetTopic(string name);
    }
}