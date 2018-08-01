using Cthoni.Core;
using Cthoni.Core.CommandLine;
using Cthoni.Core.DependencyInjection;
using JetBrains.Annotations;

namespace Cthoni
{
    public static class DependencyInjection
    {
        [NotNull] public static IFactory CreateFactory()
        {
            var factory = new Factory();

            factory.RegisterSingleton<IFactory>(factory);
            factory.RegisterSingleton<ICommandLine, CommandLine>();

            factory.Register<ICommandLineProcessor, CommandLineProcessor>();

            return factory;
        }
    }
}
