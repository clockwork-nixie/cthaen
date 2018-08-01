using System;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class Response
    {
        public Response([NotNull] string text, ResponseType type = ResponseType.Text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            Text = text;
            Type = type;
        }


        public Response(ResponseType type) : this(string.Empty, type) { }


        public static implicit operator Response([NotNull] string text) => new Response(text);


        public ResponseType Type { get; }
        [NotNull] public string Text { get; }
    }
}
