using System;
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
            Facts = factory.GetInstance<IFactBase>();
        }


        public IFactBase Facts { get; }
    }
}
