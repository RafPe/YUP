using YUP.App.Contracts;

namespace YUP.App.Services
{
    /// <summary>
    /// Interface used for communication between views
    /// </summary>
    public interface IEventBus
    {
        void PublishEvent(string name, EventBusHandler busHandler);
        void RaiseEvent(string name, object sender, EventBusArgs busArgs);
        void SubscribeEvent(string name, EventBusHandler busHandler);
    }
}
