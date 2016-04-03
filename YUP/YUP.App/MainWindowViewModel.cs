using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Videos;
using YUP.App.Yupis;

namespace YUP.App
{
    public class MainWindowViewModel : BindableBase
    {


        private BindableBase _CurrentViewModel;

        private VideosViewModel _videosViewModel;


        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            _CurrentViewModel = ContainerHelper.GetService<YupisViewModel>();
        }

        public BindableBase CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set { SetProperty(ref _CurrentViewModel, value); }
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
