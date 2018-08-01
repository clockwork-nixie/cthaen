using System;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandLineResponse
    {
        public CommandLineResponse([NotNull] string text, CommandLineResponseType type = CommandLineResponseType.Text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            Text = text;
            Type = type;
        }


        public CommandLineResponse(CommandLineResponseType type) : this(string.Empty, type) { }


        public static implicit operator CommandLineResponse([NotNull] string text) => new CommandLineResponse(text);


        public CommandLineResponseType Type { get; }
        [NotNull] public string Text { get; }
    }
}
