using System;
using System.Collections.Generic;
using System.Linq;

namespace Cthoni.Core.CommandLine.ParsePolicies
{
    public class AsymmetricParsePolicy : SimpleParsePolicy
    {
        private static readonly char[] _separators = { ' ', '\n', '\r', '\t' };

        public override IEnumerable<ParseToken> ParseInput(string sentence) => sentence
            .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t != null)
            .Select(t => new ParseToken { Text = t });
    }
}
