using System.ComponentModel;
using YUP.App.Services;

namespace YUP.App.vPlayer
{
    public class PlayerViewModel: BindableBase
    {
        private IMediaPlayer _mediaPlayer; // Hostowany obiekt flash player'a

        public string whatever { get; set; } = "kurwa";

        public PlayerViewModel()
        {
        }
        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
        }
    }
}


//var host = new WindowsFormsHost(); // Interop z Windows Forms

//applogger.Info("Flash player - init activeX control");
//            flashAxPlayer = new FlashAxControl(); // Inicjalizacja playera
//host.Child = flashAxPlayer; // nasz interop hostuje player'a

//            applogger.Info("Set h/w");
//            int h = (int)StackPanel_Player.ActualHeight - 5; // Ustawiamy wysokosc automatycznie
//int w = (int)StackPanel_Player.ActualWidth - 5; // Ustawiamy szerokosc automatycznie

//applogger.Info("Current h/w [" + h + "/" + w + "]");

//            applogger.Info("set player size (object)");
//            flashAxPlayer.Width = w;
//            flashAxPlayer.Height = h;

//            applogger.Info("set player size (javascript)");
//            flashAxPlayer.mediaSetPlayerSize(w, h); // Wywolujemy bezposrednio JavaScript na obiekcie

//            applogger.Info("Add player to UI");
//            stackpanel4player.Children.Add(host); // Dodajemy player'a do naszego stack player'a   