using JetBrains.Annotations;

namespace Cthoni.Core.DependencyInjection
{
    public interface IFactory
    {
        [NotNull] TInstance GetInstance<TInstance>() where TInstance : class;
    }
}
