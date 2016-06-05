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

        /*
            //TODO: UI-002
            This is a bit of against good practice since we 
            should not have seperated properties for each of the view 
            --- however since I switched to tabs this approach needs to be 
            revisited carefully 

        */

        private BindableBase _DashboardViewModel;
        public  BindableBase DashboardViewModel
        {
            get
            {
                return _DashboardViewModel;
            }
            set { SetProperty(ref _DashboardViewModel, value); }
        }

        private BindableBase _PlayerViewModel;
        public  BindableBase PlayerViewModel
        {
            get
            {
                return _PlayerViewModel;
            }
            set { SetProperty(ref _PlayerViewModel, value); }
        }


        private BindableBase _VideosViewModel;
        public  BindableBase VideosViewModel
        {
            get
            {
                return _VideosViewModel;
            }
            set { SetProperty(ref _VideosViewModel, value); }
        }

        private BindableBase _ChannelsViewModel;
        public  BindableBase ChannelsViewModel
        {
            get
            {
                return _ChannelsViewModel;
            }
            set { SetProperty(ref _ChannelsViewModel, value); }
        }

        private BindableBase _YuipsViewModel;
        public  BindableBase YuipsViewModel
        {
            get
            {
                return _YuipsViewModel;
            }
            set { SetProperty(ref _YuipsViewModel, value); }
        }





        public MainWindowViewModel()
        {

            _yupSettings       = ContainerHelper.GetService<IYupSettings>(); ;

            _YuipsViewModel    = ContainerHelper.GetService<YupisViewModel>();
            _VideosViewModel   = ContainerHelper.GetService<VideosViewModel>();
            _ChannelsViewModel = ContainerHelper.GetService<ChannelsViewModel>();
            _PlayerViewModel   = ContainerHelper.GetService<PlayerViewModel>();

            //_DashboardViewModel = ContainerHelper.GetService<DashboardViewModel>();
        }
    }
}
