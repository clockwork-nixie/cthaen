using System;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    public interface IDirectiveSet<TResponse>
        where TResponse : class
    {
        [NotNull] TResponse Process([NotNull] string sentence);

        // ReSharper disable UnusedMember.Global
        void Register([NotNull] string pattern, [NotNull] Action action);
        void Register([NotNull] string pattern, [NotNull] Action<string> action);
        void Register([NotNull] string pattern, [NotNull] Action<string, string> action);
        void Register([NotNull] string pattern, [NotNull] Action<string, string, string> action);
        void Register([NotNull] string pattern, [NotNull] Action<string, string, string, string> action);
        void Register([NotNull] string pattern, [NotNull] Func<TResponse> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, TResponse> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, TResponse> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, string, TResponse> action);
        void Register([NotNull] string pattern, [NotNull] Func<string, string, string, string, TResponse> action);
        // ReSharper restore UnusedMember.Global
    }
}