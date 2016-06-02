using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Dialogs
{
    class DialogEditChannelViewModel : BindableBase
    {

        private IHoldingBay _holdingBay;

        private YTChannel _NewYtChannel;
        public YTChannel NewYtChannel
        {
            get { return _NewYtChannel; }
            set
            {
                _NewYtChannel = value;

                _holdingBay.AddEntry("CHANNEL_NEW",value); // We update our existing object
            }
        }

        public DialogEditChannelViewModel()
        {
            _holdingBay = ContainerHelper.GetService<IHoldingBay>();

            NewYtChannel = (YTChannel) _holdingBay.GetEntry("CHANNEL_NEW",false); // do not remove object from our repo

        }

        public DialogEditChannelViewModel(YTChannel newYtChannel)
        {
            
        }

    }
}
