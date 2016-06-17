namespace YUP.App.Services
{
    /// <summary>
    /// Interface exposing methods used for registrations/publications
    /// </summary>
    public interface IEventRegistrator
    {
        void PublishEvents();
        void SubscribeEvents();

    }
}
