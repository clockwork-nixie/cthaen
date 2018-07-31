using JetBrains.Annotations;

namespace Cthoni.Core.Interfaces
{
    public interface IFactory
    {
        [NotNull] TInstance GetInstance<TInstance>() where TInstance : class;
    }
}
