using System;
using System.Collections.Generic;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public delegate void EventBusHandler(object sender, EventBusArgs busArgs);

    public class EventBusArgs : EventArgs
    {
        public object Item { get; set; }
    }

    public class EventBus : IEventBus
    {

        public EventBus()
        {
            
        }

        private  Dictionary<string, EventBusHandler> events =
                new Dictionary<string, EventBusHandler>();

        public void PublishEvent(string name, EventBusHandler busHandler)
        {
            if (!events.ContainsKey(name))
                events.Add(name, busHandler);
        }

        public void RaiseEvent(string name, object sender, EventBusArgs busArgs)
        {
            if (events.ContainsKey(name) && events[name] != null)
                events[name](sender, busArgs);
        }

        public void SubscribeEvent(string name, EventBusHandler busHandler)
        {
            if (events.ContainsKey(name))
                events[name] += busHandler;
        }
    }
}
