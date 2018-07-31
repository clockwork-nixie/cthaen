using Cthoni.Core.DependencyInjection;
using Cthoni.Core.Interfaces;
using Cthoni.Interfaces;
using JetBrains.Annotations;

namespace Cthoni
{
    public static class DependencyInjection
    {
        [NotNull] public static IFactory CreateFactory()
        {
            var factory = new Factory();

            factory.RegisterSingleton<ICommandLine, CommandLine>();

            return factory;
        }
    }
}
