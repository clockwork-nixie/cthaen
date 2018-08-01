using System.Collections.Generic;
using System.Linq;

namespace Cthoni.Core.CommandLine
{
    public class SimpleParsePolicy : IParsePolicy
    {
        public IEnumerable<ParseToken> ParseInput(string sentence) => ParseSpecification(sentence);


        public IEnumerable<ParseToken> ParseSpecification(string sentence) => sentence
            .Split(' ')
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => new ParseToken {
                IsParameter = t.StartsWith("$"),
                Text = t.StartsWith("$")? t.Substring(1): t
            });


        public string SafePlaceholder => "\0";
    }
}
