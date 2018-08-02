using Cthoni.Core.CommandLine;
using Cthoni.Core.CommandLine.ParsePolicies;
using Cthoni.Core.Science;
using Cthoni.Utilities;
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
            factory.RegisterSingleton<IFactBase, FactBase>();

            factory.Register<ICommandLineProcessor, CommandLineProcessor>();
            factory.Register<IDirectiveSet>(() => new DirectiveSet(new AsymmetricParsePolicy()));

            return factory;
        }
    }
}
