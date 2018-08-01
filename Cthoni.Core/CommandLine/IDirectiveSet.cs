using System;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface IDirectiveSet
    {
        [NotNull] Response Process([NotNull] string sentence);
        void Register([NotNull] string pattern, [NotNull] Func<Response> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, Response> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, Response> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, string, Response> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, string, string, Response> action);
    }
}