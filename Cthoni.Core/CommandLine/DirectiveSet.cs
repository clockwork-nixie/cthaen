using System;
using System.Linq;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class DirectiveSet
    {
        [NotNull] private readonly ParseTree _directives = new ParseTree();
        [NotNull] private readonly IParsePolicy _parser;


        public DirectiveSet([NotNull] IParsePolicy parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }
            _parser = parser;
        }


        [NotNull]
        public Response Process([NotNull] string sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException(nameof(sentence));
            }
            Response response = null;

            if (!string.IsNullOrEmpty(sentence))
            {
                var tokens = _parser.ParseInput(sentence).ToArray();
                var method = _directives.Find(tokens.Select(token => token?.Text).ToArray()).FirstOrDefault();

                response = method != null? 
                    method.Invoke(tokens.Select(t => t?.Text).ToArray()): 
                    new Response("I'm sorry, Laura; I don't understand the question.", ResponseType.NotFound);
            }
            return response ?? new Response(string.Empty);
        }


        // ReSharper disable UnusedMember.Global
        public void Register([NotNull] string pattern, [NotNull] Func<Response> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, Response> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, Response> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, string, Response> action) { Register(pattern, (Delegate)action); }
        public void Register([NotNull] string pattern, [NotNull] Func<string, string, string, string, Response> action) { Register(pattern, (Delegate)action); }
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
            var arguments = tokens
                .Select(t => new { Name = t?.Text, Index = counter++, IsParameter = t != null && t.IsParameter })
                .Where(t => t.IsParameter)
                .ToArray();

            if (arguments.Length != arguments.Select(a => a?.Name).Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().Count())
            {
                throw new ArgumentException($"Duplicate parameter found in directive: {pattern}");
            }

            var lookup = arguments.Where(a => a != null).ToDictionary(a => a.Name, a => a.Index);
            // ReSharper disable once AssignNullToNotNullAttribute
            var indices = parameters.Select(p => { int index; return lookup.TryGetValue(p?.Name, out index)? index: -1; }).ToArray();

            if (indices.Any(i => i < 0))
            {
                throw new ArgumentException($"Failed to map argments for directive: {pattern}");
            }
            Func<string[], Response> method = 
                inputs => (Response)action.DynamicInvoke(indices.Select(i => (object)(inputs?[i])).ToArray());

            _directives.Add(tokens, method);
        }
    }
}
