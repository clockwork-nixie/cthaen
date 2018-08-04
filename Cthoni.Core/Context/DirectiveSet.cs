using System;
using System.Linq;
using Cthoni.Core.CommandLine;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public class DirectiveSet<TResponse> : IDirectiveSet<TResponse>
        where TResponse : class
    {
        [NotNull] private readonly ParseTree<TResponse> _directives = new ParseTree<TResponse>();
        [NotNull] private readonly IParsePolicy _parser;
        [NotNull] private readonly Func<TResponse> _onNotFound;
        [NotNull] private readonly Func<TResponse> _onSuccess;


        public DirectiveSet([NotNull] IParsePolicy parser,
            [NotNull] Func<TResponse> onNotFound,
            [NotNull] Func<TResponse> onSuccess)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            if (onNotFound == null)
            {
                throw new ArgumentNullException(nameof(onNotFound));
            }

            if (onSuccess == null)
            {
                throw new ArgumentNullException(nameof(onSuccess));
            }
            _onNotFound = onNotFound;
            _onSuccess = onSuccess;
            _parser = parser;
        }


        public TResponse Process(string sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException(nameof(sentence));
            }
            TResponse response;

            var tokens = _parser.ParseInput(sentence).ToArray();
            var method = _directives.Find(tokens.Select(token => token?.Text).ToArray()).FirstOrDefault();

            response = method != null?
                method.Invoke(tokens.Select(t => t?.Text).ToArray()):
                _onNotFound();

            if (response == null)
            {
                throw new CommandLineException($"Processing sentence returned null result: {sentence}");
            }
            return response;
        }


        // ReSharper disable UnusedMember.Global
        public void Register(string pattern, Action action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Action<string> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Action<string, string> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Action<string, string, string> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Action<string, string, string, string> action) => Register(pattern, (Delegate)action);

        public void Register(string pattern, Func<TResponse> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Func<string, TResponse> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Func<string, string, TResponse> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Func<string, string, string, TResponse> action) => Register(pattern, (Delegate)action);
        public void Register(string pattern, Func<string, string, string, string, TResponse> action) => Register(pattern, (Delegate)action);
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
            var method = (action.Method.ReturnType == typeof(void)) ?
                (inputs => { action.DynamicInvoke(indices.Select(i => (object)(inputs?[i])).ToArray()); return _onSuccess(); }) :
                (Func<string[], TResponse>)(inputs => (TResponse)action.DynamicInvoke(indices.Select(i => (object)(inputs?[i])).ToArray()));

            _directives.Add(tokens, method);
        }
    }
}
