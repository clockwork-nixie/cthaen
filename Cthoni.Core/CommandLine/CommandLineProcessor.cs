using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    [UsedImplicitly]
    public class CommandLineProcessor : ICommandLineProcessor
    {
        [NotNull] private readonly CommandLineDirectiveSet _directives = 
            new CommandLineDirectiveSet(new SimpleParsePolicy());


        public CommandLineProcessor()
        {
            _directives.Register("hello", () => "yo!");
            _directives.Register("my name is $name", name => $"Pleased to meet you, {name}.");
            _directives.Register("quit", () => new CommandLineResponse(CommandLineResponseType.Quit));
        }


        public CommandLineResponse Process(string command)
        {
            CommandLineResponse response;

            try
            {
                response = _directives.Process(command);
            }
            catch (CommandLineException exception)
            {
                response = new CommandLineResponse(
                    exception.Message ?? "Unknown error.",
                    CommandLineResponseType.Error);
            }
            return response;
        } 
    }
}
