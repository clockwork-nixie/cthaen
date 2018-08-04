using System.Collections.Generic;
using System.Text;
using Cthoni.Utilities;

namespace Cthoni.Core.Context.ParsePolicies
{
    public class MultiWordParsePolicy : SimpleParsePolicy
    {
        public override IEnumerable<ParseToken> ParseInput(string sentence)
        {
            var tokens = new List<ParseToken>();
            var buffer = new StringBuilder();
            var isEscape = false;
            var isParameter = false;
            var isToken = false;

            foreach (var character in sentence)
            {
                if (!isToken)
                {
                    if (char.IsWhiteSpace(character))
                    {
                        continue;
                    }
                    isToken = true;

                    if (character == '"')
                    {
                        isParameter = true;
                        continue;
                    }
                }

                if (isEscape)
                {
                    isEscape = false;
                    buffer.Append(character);
                }
                else if (character == '\\')
                {
                    isEscape = true;
                }
                else if (isParameter)
                {
                    if (character == '"')
                    {
                        tokens.Add(new ParseToken {
                            IsParameter = true,
                            Text = buffer.ToString()
                        });

                        isParameter = false;
                        isToken = false;
                        buffer.Clear();
                    }
                    buffer.Append(character);
                }
                else if (char.IsWhiteSpace(character))
                {
                    tokens.Add(new ParseToken { Text = buffer.ToString() });
                    isToken = false;
                    buffer.Clear();
                }
                else
                {
                    buffer.Append(character);
                }
            }

            if (isToken)
            {
                if (isParameter)
                {
                    throw new InputException("Unterminated quoted string.");
                }
                tokens.Add(new ParseToken { Text = buffer.ToString() });
            }
            return tokens;
        }
    }
}
