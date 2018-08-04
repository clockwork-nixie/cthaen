using Cthoni.Core.Context;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public interface ICommandLineDirectives
    {
        void Populate([NotNull] ICommandSet commandSet, [NotNull] IContext context);
    }
}