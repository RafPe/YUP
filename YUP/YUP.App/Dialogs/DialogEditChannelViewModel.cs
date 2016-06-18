using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;
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
        
        public ObservableCollection<string> ocChannelTags { get; set; } 

        public string txtTag { get; set; }

        public DialogEditChannelViewModel()
        {
            RegisterRelayCommand();

            ocChannelTags = new ObservableCollection<string>();

            this.useHoldingBay = true;

            _holdingBay = ContainerHelper.GetService<IHoldingBay>();

            NewYtChannel = (YTChannel) _holdingBay.GetEntry(HoldingBayItem.CHANNEL_NEW, false); // do not remove object from our repo

        }

        public DialogEditChannelViewModel(YTChannel newYtChannel)
        {
            RegisterRelayCommand();

            ocChannelTags = new ObservableCollection<string>();

            NewYtChannel = newYtChannel;

        }

        /// <summary>
        /// With this method we register all our RelayCommands 
        /// Useful when having more than 1 constructor
        /// </summary>
        private void RegisterRelayCommand()
        {
            cmdAddChannelTag = new RelayCommand(onCmdAddChannelTag);
        }

        private async void onCmdAddChannelTag()
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DialogAddTag()
            {
                DataContext = this
            };

            //show the dialog
            bool result = (bool)await DialogHost.Show(view, DialogHostId.CHANNEL_EDIT_TAGS);

            if (!ReferenceEquals(null, txtTag))
            {   
                NewYtChannel.tags.Add(txtTag);

                ocChannelTags.Add(txtTag);

                txtTag = "";
            }

        }
    }
}
