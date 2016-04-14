using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Autofac;
using YUP.App.Contracts;
using YUP.App.MediaPlayers;
using YUP.App.Services;

namespace YUP.App.Player
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private FlashAxControl  cc; // Hostowany obiekt flash player'a
        private IEventBus       _eventBus;


        public PlayerView()
        {
            _eventBus = ContainerHelper.GetService<IEventBus>();

            _eventBus.SubscribeEvent("VideoIdChanged", VideoIdChangedHandler);

            InitializeComponent();


        }

        private void VideoIdChangedHandler(object sender, EventBusArgs busargs)
        {
            var test = (string) busargs.Item;

            cc.mediaLoadVideo(test);
        }

        private void PlayerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var host = new WindowsFormsHost(); // Interop z Windows Forms

            cc = (FlashAxControl)ContainerHelper.GetService<IMediaPlayer>("youtube");

            host.Child = cc;
            int Height = (int) player_youtube.ActualHeight;
            int Width = (int) player_youtube.ActualWidth;

            cc.Height = Height;
            cc.Width = Width;

            cc.mediaSetPlayerSize(Width, Height);


            player_youtube.Children.Add(host);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}


//var yyy = ContainerHelper.GetService<IMediaPlayer>("youtube");
//yyy.mediaLoadVideo("b9FC9fAlfQE");