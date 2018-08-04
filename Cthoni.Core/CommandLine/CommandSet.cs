using Cthoni.Core.Context;
using JetBrains.Annotations;

namespace Cthoni.Core.CommandLine
{
    public class CommandSet : DirectiveSet<Response>, ICommandSet
    {
        public CommandSet([NotNull] IParsePolicy parser)
            : base(parser, () => ResponseType.NotFound, () => ResponseType.Ok)
        {
            // EMPTY
        }
    }
}
