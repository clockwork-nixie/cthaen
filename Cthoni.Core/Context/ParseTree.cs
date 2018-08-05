using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public class ParseTree<TResponse>
        where TResponse : class
    {
        #region ParseNode
        private class ParseNode
        {
            [NotNull] public IDictionary<string, ParseNode> Children { get; } = new Dictionary<string, ParseNode>();
            public Func<string[], TResponse> Method { get; set; }
        }
        #endregion


        [NotNull] private readonly ParseNode _root = new ParseNode();


        public void Add([NotNull, ItemNotNull] ParseToken[] tokens, [NotNull] Func<string[], TResponse> method)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (tokens.Length == 0)
            {
                throw new ArgumentException("Token sequence cannot be empty.");
            }
            AddRecursive(tokens, method, _root, 0);
        }


        private static void AddRecursive(
            [NotNull, ItemNotNull] IReadOnlyList<ParseToken> tokens,
            [NotNull] Func<string[], TResponse> method,
            [NotNull] ParseNode current,
            int depth)
        {
            var token = tokens[depth];

            if (string.IsNullOrWhiteSpace(token?.Text))
            {
                throw new ArgumentException("Empty tokens not permitted in pattern.");
            }
            var key = token.IsParameter? string.Empty: token.Text;
            ParseNode next;

            if (!current.Children.TryGetValue(key, out next))
            {
                next = new ParseNode();
                current.Children[key] = next;
            }

            if (depth == tokens.Count - 1)
            {
                if (next.Method != null)
                {
                    throw new DuplicateNameException("Method already registered.");
                }
                next.Method = method;
                return;
            }
            AddRecursive(tokens, method, next, depth + 1);
        }


        [NotNull, ItemNotNull]
        public IEnumerable<Func<string[], TResponse>> Find(
            [NotNull, ItemNotNull] IReadOnlyList<string> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            var methods = new List<Func<string[], TResponse>>();

            FindRecursive(tokens, methods, _root, 0);

            return methods;
        }


        private static void FindRecursive(
            [NotNull, ItemNotNull] IReadOnlyList<string> tokens,
            [NotNull] List<Func<string[], TResponse>> methods,
            [NotNull] ParseNode current,
            int depth)
        {
            var token = tokens[depth];

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Empty tokens not permitted in sequence.");
            }
            ParseNode next;

            if (current.Children.TryGetValue(token, out next) && next != null)
            {
                if (depth == tokens.Count - 1)
                {
                    if (next.Method != null)
                    {
                        methods.Add(next.Method);
                    }
                }
                else
                {
                    FindRecursive(tokens, methods, next, depth + 1);
                }
            }
            
            if (current.Children.TryGetValue(string.Empty, out next) && next != null)
            {
                if (depth == tokens.Count - 1)
                {
                    if (next.Method != null)
                    {
                        methods.Add(next.Method);
                    }
                }
                else
                {
                    FindRecursive(tokens, methods, next, depth + 1);
                }
            }
        }
    }
}
