using YUP.App.Contracts;
using YUP.App.Player;
using YUP.App.Services;
using YUP.App.vChannels;
using YUP.App.Videos;
using YUP.App.Yupis;

namespace YUP.App
{
    public class MainWindowViewModel : BindableBase
    {


        private BindableBase _currentDetailsViewModel;
        public  BindableBase CurrentDetailsViewModel
        {
            get
            {
                return _currentDetailsViewModel;
            }
            set { SetProperty(ref _currentDetailsViewModel, value); }
        }

        private BindableBase _CurrentDashboardViewModel;
        public  BindableBase CurrentDashboardViewModel
        {
            get
            {
                return _CurrentDashboardViewModel;
            }
            set { SetProperty(ref _CurrentDashboardViewModel, value); }
        }



        private IYupSettings _yupSettings;


        public MainWindowViewModel()
        {

            _yupSettings                = ContainerHelper.GetService<IYupSettings>(); ;

            NavCommand                  = new RelayCommand<string>(OnNav);


            _currentDetailsViewModel    = ContainerHelper.GetService<YupisViewModel>();
            _CurrentDashboardViewModel  = ContainerHelper.GetService<PlayerViewModel>();
        }



        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "yupis":
                    CurrentDetailsViewModel = ContainerHelper.GetService<YupisViewModel>();
                    break;
                case "videos":
                    CurrentDetailsViewModel = ContainerHelper.GetService<VideosViewModel>();
                    break;
                case "channels":
                    CurrentDashboardViewModel = ContainerHelper.GetService<ChannelsViewModel>();
                    break;
                default:
                    break;
            }

        }

    }
}
