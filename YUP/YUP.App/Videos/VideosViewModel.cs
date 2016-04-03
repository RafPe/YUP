using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Services;

namespace YUP.App.Videos
{
    public class VideosViewModel : BindableBase
    {
        private IYtManager _ytManager;


        public VideosViewModel(IYtManager ytManager)
        {
            _ytManager = ytManager;
        }

        public async void OnLoad()
        {

        }

    }

}
