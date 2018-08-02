using System;
using JetBrains.Annotations;
using SimpleInjector;

namespace Cthoni.Utilities
{
    public class Factory : IFactory
    {
        [NotNull] private readonly Container _container = new Container();


        public TInstance GetInstance<TInstance>()
            where TInstance : class
        {
            var instance = _container.GetInstance<TInstance>();

            if (instance == null)
            {
                throw new InvalidOperationException($"Creating an instance of {typeof(TInstance).FullName} yielded a null object.");
            }
            return instance;
        }


        // ReSharper disable once UnusedMember.Global
        public void Register<TConcrete>() where TConcrete : class
        {
            _container.Register<TConcrete>();
        }


        // ReSharper disable once UnusedMember.Global
        public void Register<TAbstract>(Func<TAbstract> factoryMethod) where TAbstract : class
        {
            _container.Register(factoryMethod);
        }


        // ReSharper disable once UnusedMember.Global
        public void Register<TAbstract, TConcrete>() where TAbstract : class where TConcrete : class, TAbstract
        {
            _container.Register<TAbstract, TConcrete>();
        }


        // ReSharper disable once UnusedMember.Global
        public void RegisterSingleton<TConcrete>() where TConcrete : class
        {
            _container.RegisterSingleton<TConcrete>();
        }


        // ReSharper disable once UnusedMember.Global
        public void RegisterSingleton<TConcrete>(TConcrete instance) where TConcrete : class
        {
            _container.RegisterInstance(instance);
        }


        // ReSharper disable once UnusedMember.Global
        public void RegisterSingleton<TAbstract>(Func<TAbstract> factoryMethod) where TAbstract : class
        {
            _container.RegisterSingleton(factoryMethod);
        }


        // ReSharper disable once UnusedMember.Global
        public void RegisterSingleton<TAbstract, TConcrete>() where TAbstract : class where TConcrete : class, TAbstract
        {
            _container.RegisterSingleton<TAbstract, TConcrete>();
        }


        // ReSharper disable once UnusedMember.Global
        public void Verify()
        {
            _container.Verify();
        }
    }
}
