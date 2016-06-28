using YUP.App.Services;
using YUP.App.vChannels;
using YUP.App.vPlayer;
using YUP.App.vVideos;
using YUP.App.vYupis;

namespace YUP.App
{
    public class MainWindowViewModel : BindableBase
    {

        private IYupSettings _yupSettings;

        #region Tab views

        private BindableBase _DashboardViewModel;
        public BindableBase DashboardViewModel
        {
            get
            {
                return _DashboardViewModel;
            }
            set { SetProperty(ref _DashboardViewModel, value); }
        }

        private BindableBase _PlayerViewModel;
        public BindableBase PlayerViewModel
        {
            get
            {
                return _PlayerViewModel;
            }
            set { SetProperty(ref _PlayerViewModel, value); }
        }


        private BindableBase _VideosViewModel;
        public BindableBase VideosViewModel
        {
            get
            {
                return _VideosViewModel;
            }
            set { SetProperty(ref _VideosViewModel, value); }
        }

        private BindableBase _ChannelsViewModel;
        public BindableBase ChannelsViewModel
        {
            get
            {
                return _ChannelsViewModel;
            }
            set { SetProperty(ref _ChannelsViewModel, value); }
        }

        private BindableBase _YuipsViewModel;
        public BindableBase YuipsViewModel
        {
            get
            {
                return _YuipsViewModel;
            }
            set { SetProperty(ref _YuipsViewModel, value); }
        }

        #endregion

        #region Single View

        private BindableBase _MainAppViewModel;
        public BindableBase MainAppViewModel
        {
            get
            {
                return _MainAppViewModel;
            }
            set { SetProperty(ref _MainAppViewModel, value); }
        }

        #endregion

        public RelayCommand<string> NavCommand
        {
            get;
            private set;
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "yupis":
                    MainAppViewModel = ContainerHelper.GetService<YupisViewModel>();
                    break;
                case "videos":
                    MainAppViewModel = ContainerHelper.GetService<VideosViewModel>();
                    break;
                case "channels":
                    MainAppViewModel = ContainerHelper.GetService<ChannelsViewModel>();
                    break;
                case "player":
                    MainAppViewModel = ContainerHelper.GetService<PlayerViewModel>();
                    break;
                default:
                    break;
            }

        }



        public MainWindowViewModel()
        {

            _yupSettings       = ContainerHelper.GetService<IYupSettings>(); ;

            _YuipsViewModel    = ContainerHelper.GetService<YupisViewModel>();
            _VideosViewModel   = ContainerHelper.GetService<VideosViewModel>();
            _ChannelsViewModel = ContainerHelper.GetService<ChannelsViewModel>();
            _PlayerViewModel   = ContainerHelper.GetService<PlayerViewModel>();


            NavCommand = new RelayCommand<string>(OnNav);
            _MainAppViewModel = ContainerHelper.GetService<PlayerViewModel>();
        }
    }
}
