using Autofac;
using YUP.Core.Services;

namespace YUP.Player.Youtube
{
    /// <summary>
    /// Autofac class used for dynamic registration of services in main application
    /// </summary>
    public class YoutubePlayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FlashAxControl>().Named<IPlayer>("youtube").As<IPlayer>();
        }
    }
}
