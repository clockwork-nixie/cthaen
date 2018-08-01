using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine.ParsePolicies
{
    public class AsymmetricParsePolicy : SimpleParsePolicy
    {
        private static readonly char[] _separators = { ' ', '\n', '\r', '\t' };
        [NotNull] private readonly string _prefix;


        public override IEnumerable<ParseToken> ParseInput(string sentence) => sentence
            .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t != null)
            .Select(t => new ParseToken { Text = t });
    }
}
