using System;
using JetBrains.Annotations;

namespace Cthoni.Core.DependencyInjection
{
    public interface IFactoryBuilder
    {
        void Register<TConcrete>() where TConcrete : class;
        void Register<TAbstract>([NotNull] Func<TAbstract> factoryMethod) where TAbstract : class;
        void Register<TAbstract, TConcrete>() where TAbstract : class where TConcrete : class, TAbstract;

        void RegisterSingleton<TConcrete>() where TConcrete : class;
        void RegisterSingleton<TConcrete>([NotNull] TConcrete instance) where TConcrete : class;
        void RegisterSingleton<TAbstract>([NotNull] Func<TAbstract> factoryMethod) where TAbstract : class;
        void RegisterSingleton<TAbstract, TConcrete>() where TAbstract : class where TConcrete : class, TAbstract;

        void Verify();
    }
}
