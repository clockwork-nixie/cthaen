using Cthoni.Core.CommandLine;
using Cthoni.Core.Context;
using Cthoni.Core.Context.ParsePolicies;
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

            factory.Register<IFactory>(() => factory);

            factory.Register<ICommandLineConsole, CommandLineConsole>();
            factory.Register<ICommandLineDirectives, CommandLineDirectives>();
            factory.Register<ICommandLineProcessor, CommandLineProcessor>();
            factory.Register<ICommandSet>(() => new CommandSet(new AsymmetricParsePolicy()));
            factory.Register<IContext, Context>();
            factory.Register<IFactBase, FactBase>();
            
            return factory;
        }
    }
}
