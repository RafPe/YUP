using YUP.App.Player;
using YUP.App.Videos;
using YUP.App.Yupis;

namespace YUP.App
{
    public class MainWindowViewModel : BindableBase
    {


        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        private BindableBase _CurrentPlayerViewModel;
        public BindableBase CurrentPlayerViewModel
        {
            get
            {
                return _CurrentPlayerViewModel;
            }
            set { SetProperty(ref _CurrentPlayerViewModel, value); }
        }


        public MainWindowViewModel()
        {
            NavCommand                  = new RelayCommand<string>(OnNav);
            _CurrentViewModel           = ContainerHelper.GetService<YupisViewModel>();
            _CurrentPlayerViewModel     = ContainerHelper.GetService<PlayerViewModel>();
        }



        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "yupis":
                    //CurrentViewModel = new YupisViewModel();
                    CurrentViewModel = ContainerHelper.GetService<YupisViewModel>();
                    break;
                case "videos":
                    CurrentViewModel = ContainerHelper.GetService<VideosViewModel>();
                    break;
                default:
                    break;
            }

        }

    }
}
