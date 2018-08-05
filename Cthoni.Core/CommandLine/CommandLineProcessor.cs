using System;
using Cthoni.Core.Context;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly ICommandSet _commandSet;


        public CommandLineProcessor([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _commandSet = factory.GetInstance<ICommandSet>();

            Context = factory.GetInstance<IContext>();

            var builder = factory.GetInstance<ICommandLineDirectives>();
            
            builder.Populate(_commandSet, Context);
        }


        public IContext Context { get; }


        public Response Process(string command) => _commandSet.Process(command);
    }
}
