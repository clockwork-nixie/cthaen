using Cthoni.Core.CommandLine;
using Cthoni.Core.CommandLine.ParsePolicies;
using Cthoni.Core.Context;
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

            factory.Register<ICommandLine, CommandLine>();
            factory.Register<ICommandLineProcessor, CommandLineProcessor>();
            factory.Register<IContext, Context>();
            factory.Register<IDirectiveSet<Response>>(() => new DirectiveSet<Response>(
                new AsymmetricParsePolicy(), () => ResponseType.NotFound, () => ResponseType.Ok));
            factory.Register<IFactBase, FactBase>();
            
            return factory;
        }
    }
}
