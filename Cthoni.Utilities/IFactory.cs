using JetBrains.Annotations;

namespace Cthoni.Utilities
{
    public interface IFactory
    {
        [NotNull] TInstance GetInstance<TInstance>() where TInstance : class;
    }
}
