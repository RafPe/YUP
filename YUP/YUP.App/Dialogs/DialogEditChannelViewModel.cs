using System;
using YUP.App.Base;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Dialogs
{
    class DialogEditChannelViewModel : BindableBase
    {

        private IHoldingBay _holdingBay;
        private bool        useHoldingBay = false;

        private YTChannel _NewYtChannel;
        public YTChannel NewYtChannel
        {
            get { return _NewYtChannel; }
            set
            {
                _NewYtChannel = value;

                if(useHoldingBay) _holdingBay.AddEntry(HoldingBayItem.CHANNEL_NEW, value); // We update our existing object
            }
        }

        public RelayCommand cmdAddChannelTag { get; private set; }

        public DialogEditChannelViewModel()
        {
            this.useHoldingBay = true;

            _holdingBay = ContainerHelper.GetService<IHoldingBay>();

            NewYtChannel = (YTChannel) _holdingBay.GetEntry(HoldingBayItem.CHANNEL_NEW, false); // do not remove object from our repo

        }

        public DialogEditChannelViewModel(YTChannel newYtChannel)
        {
            NewYtChannel = newYtChannel;
        }


        private void RegisterRelayCommand()
        {
            cmdAddChannelTag = new RelayCommand(onCmdAddChannelTag);
        }

        private void onCmdAddChannelTag()
        {

        }
    }
}
