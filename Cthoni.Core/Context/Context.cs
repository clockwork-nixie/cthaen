using System;
using Cthoni.Core.CommandLine;
using Cthoni.Core.Science;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    [UsedImplicitly]
    public class Context : IContext
    {
        public Context([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            Directives = factory.GetInstance<IDirectiveSet<Response>>();
            Facts = factory.GetInstance<IFactBase>();
        }


        public IDirectiveSet<Response> Directives { get; }
        public IFactBase Facts { get; }
    }
}
