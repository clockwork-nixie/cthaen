using System;
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
            _directives.Register("hello", (Func<string>)(() => "Pleased to meet you."));
            _directives.Register("my name is $name", (Func<string, string>)(name => $"Pleased to meet you, {name}."));
        }


        public string Process(string command) => _directives.Process(command);
    }
}
