using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandLineDirectiveSet
    {
        [NotNull] private readonly IDictionary<string, Func<string[], CommandLineResponse>> _directives = 
            new Dictionary<string, Func<string[], CommandLineResponse>>();
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
        public CommandLineResponse Process([NotNull] string sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException(nameof(sentence));
            }
            CommandLineResponse response = null;

            if (!string.IsNullOrEmpty(sentence))
            {
                var tokens = _parser.ParseInput(sentence).ToArray();
                Func<string[], CommandLineResponse> method;

                if (!_directives.TryGetValue(MakeKey(tokens), out method) || method == null)
                {
                    throw new CommandLineException("I'm sorry, Laura; I don't understand the question.");
                }
                response = method.Invoke(tokens.Select(t => t.Text).ToArray());
            }
            return response ?? new CommandLineResponse(string.Empty);
        }


        // ReSharper disable UnusedMember.Global
        public void Register([NotNull] string pattern, [NotNull] Func<CommandLineResponse> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, CommandLineResponse> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, CommandLineResponse> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, string, CommandLineResponse> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, string, string, CommandLineResponse> action) { Register(pattern, (Delegate)action); }
        // ReSharper restore UnusedMember.Global


        private void Register([NotNull] string pattern, [NotNull] Delegate action)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var parameters = action.Method.GetParameters();
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

            Func<string[], CommandLineResponse> method = arguments => 
                (CommandLineResponse)action.DynamicInvoke(parameterIndices.Select(i => (object)(arguments?[i])).ToArray());

            _directives.Add(MakeKey(tokens), method);
        }
    }
}
