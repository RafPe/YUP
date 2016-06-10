using System.ComponentModel;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.Services;

namespace YUP.App.vPlayer
{
    public class PlayerViewModel: BindableBase , IEventRegistrator
    {
        private IMediaPlayer        _mediaPlayer; // Hostowany obiekt flash player'a
        private IYupRepository      _yupRepository;
        private IYtManager          _ytManager;
        private IEventBus           _eventBus;

        public event EventBusHandler VideoIdChangedHandler;

        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        public string SearchBoxTerm { get; set; } = "";

        public RelayCommand SearchBoxCmd { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus)
        {
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;
            _eventBus       = eventbus;


            SearchBoxCmd = new RelayCommand(onSearchBoxCmd);
        }


        private void onSearchBoxCmd()
        {
           //var videoId2play =  _ytManager.GetVideoIdFromUrl(SearchBoxTerm);

           // if (videoId2play == null) return;

            //_eventBus.RaiseEvent(EventOnBus.videoIdChanged, this, new EventBusArgs() { Item = SearchBoxTerm });
            
        }

        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _eventBus.RaiseEvent(EventOnBus.videoIdChanged, this, new EventBusArgs() { Item = "" });
        }



        public void PublishEvents()
        {
            _eventBus.PublishEvent(EventOnBus.videoIdChanged, VideoIdChangedHandler);
        }

        public void SubscribeEvents()
        {
            throw new System.NotImplementedException();
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