using System;
using System.Collections.Generic;
using System.Linq;

namespace Cthoni.Core.CommandLine
{
    public class SimpleParsePolicy : IParsePolicy
    {
        private static readonly char[] _separators = { ' ', '\n', '\r', '\t' };


        public IEnumerable<ParseToken> ParseInput(string sentence) => ParseSpecification(sentence);


        public IEnumerable<ParseToken> ParseSpecification(string sentence) => sentence
            .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t != null)
            .Select(t => new ParseToken { IsParameter = t.StartsWith("$"), Text = t.StartsWith("$")? t.Substring(1): t });


        public string SafePlaceholder => "\0";
    }
}
