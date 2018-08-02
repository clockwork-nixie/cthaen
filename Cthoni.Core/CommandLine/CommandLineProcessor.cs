using System;
using Cthoni.Core.Science;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly IDirectiveSet _directives;
        [NotNull] private readonly IFactBase _facts;


        public CommandLineProcessor([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _facts = factory.GetInstance<IFactBase>();
            _directives = factory.GetInstance<IDirectiveSet>();
            
            _directives.Register("hello", () => "yo!");
            _directives.Register("my name is $name", name => $"Pleased to meet you, {name}.");
            _directives.Register("quit", () => new Response(ResponseType.Quit));
        }


        public Response Process(string command)
        {
            Response response;

            try
            {
                response = _directives.Process(command);
            }
            catch (CommandLineException exception)
            {
                response = new Response(exception.Message ?? "Unknown error.", ResponseType.Error);
            }
            return response;
        } 
    }
}
