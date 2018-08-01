using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly DirectiveSet _directives = 
            new DirectiveSet(new SimpleParsePolicy());


        public CommandLineProcessor()
        {
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
