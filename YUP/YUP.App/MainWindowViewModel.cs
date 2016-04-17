using YUP.App.Contracts;
using YUP.App.Player;
using YUP.App.Services;
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

        private BindableBase _CurrentPlayerViewModel;
        public  BindableBase CurrentPlayerViewModel
        {
            get
            {
                return _CurrentPlayerViewModel;
            }
            set { SetProperty(ref _CurrentPlayerViewModel, value); }
        }

        private IYupSettings _yupSettings;


        public MainWindowViewModel()
        {

            _yupSettings                = ContainerHelper.GetService<IYupSettings>(); ;

            NavCommand                  = new RelayCommand<string>(OnNav);


            _currentDetailsViewModel    = ContainerHelper.GetService<YupisViewModel>();
            _CurrentPlayerViewModel     = ContainerHelper.GetService<PlayerViewModel>();
        }



        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "yupis":
                    //CurrentViewModel = new YupisViewModel();
                    CurrentDetailsViewModel = ContainerHelper.GetService<YupisViewModel>();
                    break;
                case "videos":
                    CurrentDetailsViewModel = ContainerHelper.GetService<VideosViewModel>();
                    break;
                default:
                    break;
            }

        }

    }
}
