using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandLineResponse
    {
        public CommandLineResponse([NotNull] string text, CommandLineResponseType type = CommandLineResponseType.Text)
        {
            Text = text;
            Type = type;
        }


        public static implicit operator CommandLineResponse([NotNull] string text) => new CommandLineResponse(text);


        public CommandLineResponseType Type { get; }
        [NotNull] public string Text { get; }
    }
}
