using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using YUP.App.MediaPlayers;
using YUP.App.Services;

namespace YUP.App.Player
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private FlashAxControl cc; // Hostowany obiekt flash player'a

        

        public PlayerView()
        {
            InitializeComponent();

            var host = new WindowsFormsHost(); // Interop z Windows Forms

            cc = (FlashAxControl)ContainerHelper.GetService<IMediaPlayer>("youtube");

            host.Child = cc;
            int Height = 300;
            int Width = 300;

            cc.Height = Height;
            cc.Width = Width;

            cc.mediaSetPlayerSize(Width, Height);
            

            player_youtube.Children.Add(host);
        }

        private void PlayerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            cc.mediaLoadVideo("b9FC9fAlfQE");
        }
    }
}