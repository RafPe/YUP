using System.Windows;
using Autofac;
using YUP.App.Contracts;
using YUP.App.MediaPlayers;
using YUP.App.Player;
using YUP.App.Services;
using YUP.App.Videos;
using YUP.App.Yupis;

namespace YUP.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// OnStartup override to initialize our DI class
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create our DI container and build it 
            ContainerHelper.InitializeBuilder();

            ContainerHelper.Builder.RegisterType<YupManger>().As<IYupiManager>();
            ContainerHelper.Builder.RegisterType<YtManager>().As<IYtManager>();

            //TODO: Register players named ? 
            ContainerHelper.Builder.RegisterType<FlashAxControl>().Named<IMediaPlayer>("youtube").SingleInstance();

            //TODO: Think if we want to register ViewModel classes as singleton instances ?!
            ContainerHelper.Builder.RegisterType<VideosViewModel>().SingleInstance();
            ContainerHelper.Builder.RegisterType<YupisViewModel>().SingleInstance();
            ContainerHelper.Builder.RegisterType<PlayerViewModel>().SingleInstance();
            //TODO: Register all external services here .....
            ContainerHelper.SetAutofacContainer();

            // Create our main window - since we removed startup URI
            YUP.App.MainWindow wnd = new MainWindow();
            wnd.Show();


        }
    }
}

//If we need to think of dependency resolving here .....


//ContainerBuilder builder = new ContainerBuilder();

//builder.Register(ctx => new EFContext())
//            .InstancePerLifetimeScope();

//builder.Register<FirstService>(c => new FirstService(c.Resolve<EFContext>()));
//builder.Register<SecondService>(c => new SecondService(c.Resolve<EFContext>()));
//builder.Register<OtherService>(c => new OtherService(c.Resolve<EFContext>()));

//builder.Register<FirstViewModel>(c => 
//            new FirstViewModel(
//                c.Resolve<FirstService>(), 
//                c.Resolve<SecondService>(), 
//                c.Resolve<OtherService>()
//            ));
//builder.Register<SecondViewModel>(c => 
//            new SecondViewModel(
//                c.Resolve<FirstService>(), 
//                c.Resolve<SecondService>(), 
//                c.Resolve<OtherService>()
//            ));
