using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandLineDirectiveSet
    {
        [NotNull] private readonly IDictionary<string, Func<string[], string>> _directives = new Dictionary<string, Func<string[], string>>();
        [NotNull] private readonly IParsePolicy _parser;


        public CommandLineDirectiveSet([NotNull] IParsePolicy parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }
            _parser = parser;
        }


        [NotNull] private string MakeKey([NotNull, ItemNotNull] IEnumerable<ParseToken> tokens) 
            => string.Join(_parser.SafePlaceholder, tokens.Select(t => t.IsParameter? string.Empty: t.Text));


        [NotNull]
        public string Process([NotNull] string sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException(nameof(sentence));
            }
            string response;

            if (string.IsNullOrEmpty(sentence))
            {
                response = string.Empty;
            }
            else
            {
                var tokens = _parser.ParseInput(sentence).ToArray();
                Func<string[], string> method;

                if (!_directives.TryGetValue(MakeKey(tokens), out method) || method == null)
                {
                    throw new CommandLineException("I'm sorry, Laura; I don't understand the question.");
                }
                response = method.Invoke(tokens.Select(t => t.Text).ToArray()) ?? string.Empty;
            }
            return response;
        }


        public void Register([NotNull] string pattern, [NotNull] Delegate action)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (action.Method.ReturnParameter?.ParameterType != typeof(string))
            {
                throw new ArgumentException("Action must be a delegate which returns a string.");
            }

            var parameters = action.Method.GetParameters();

            if (parameters.Any(p => p?.ParameterType != typeof(string)))
            {
                throw new ArgumentException("Action must be a delegate taking only string arguments.");
            }           
            
            var tokens = _parser.ParseSpecification(pattern).ToArray();
            var counter = 0;
            var parameterIndices = tokens
                .Select(t => new { Index = counter++, IsParameter = t != null && t.IsParameter })
                .Where(t => t.IsParameter)
                .Select(p => p.Index)
                .ToArray();

            if (parameterIndices.Length != parameters.Length)
            {
                throw new ArgumentException($"Input has {parameterIndices.Length} parameters but action requires {parameters.Length}.");
            }

            Func<string[], string> method = arguments => 
                (string)action.DynamicInvoke(parameterIndices.Select(i => (object)(arguments?[i])).ToArray());

            _directives.Add(MakeKey(tokens), method);
        }
    }
}
