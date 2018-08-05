using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cthoni.Core.Science;
using Cthoni.Utilities;
using JetBrains.Annotations;

namespace Cthoni.Core.Context
{
    [UsedImplicitly]
    public class Context : IContext
    {
        [NotNull] private readonly IFactory _factory;
        [NotNull] private readonly IDictionary<string, ITopic> _topics = new ConcurrentDictionary<string, ITopic>();


        public Context([NotNull] IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            _factory = factory;

            CurrentTopic = CreateTopic(string.Empty);
        }


        public ITopic CreateTopic(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var topic = _factory.GetInstance<ITopic>();
            
            topic.Initialise(name, _topics.Add);

            return topic;
        }


        public ITopic CurrentTopic { get; private set; }


        public void SetTopic(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            ITopic topic;

            if (!_topics.TryGetValue(name, out topic) || topic == null)
            {
                throw new StateException($"Topic does not exist: {name}");
            }
            CurrentTopic = topic;
        }
    }
}
