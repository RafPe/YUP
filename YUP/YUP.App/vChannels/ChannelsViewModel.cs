using System.Collections.ObjectModel;
using System.Windows;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.Helpers;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vChannels
{
    public class ChannelsViewModel : BindableBase, IEventRegistrator
    {
        private IYupRepository  _yupRepository;
        private YTChannel       _selectedYtChannel;
        private IYtManager      _ytManager;
        private IEventBus       _eventBus;


        public ObservableCollection<string>     test        { get; set; }
        public ObservableCollection<YTChannel>  YtChannels  { get; set; }


        public event EventBusHandler channelAdded;
        public event EventBusHandler channelRemoved;



        public YTChannel SelectedYtChannel
        {
            get
            {
                return _selectedYtChannel;
            }
            set
            {
                _selectedYtChannel = value;
            }
        }


        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        public string testos { get;set; } = "xyz";
        public string testos2 { get; set; } = "xyz";

        public RelayCommand klikos              { get; private set; }
        public RelayCommand cmdRemoveChannel    { get; private set; }


        public ChannelsViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus)
        {
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;
            _eventBus       = eventbus;

            YtChannels = new ObservableCollection<YTChannel>();

            YtChannels.AddRange(_yupRepository.ytChannels);

            klikos = new RelayCommand(klinkal);
            cmdRemoveChannel = new RelayCommand(onCmdRemoveChannel);

        }

        public void onCmdRemoveChannel()
        {
            YtChannels.Remove(SelectedYtChannel);
        }

        public async void klinkal()
        {

            var zmienna = await _ytManager.GetChannelIdForUserAsync(testos2);

            var x = _ytManager.GetChannelStatistcs(testos2);

            var chann = new  YTChannel()
            {
                channelId = "Electronics vBlog",
                channelFriendlyName = "Electronics blog",
                channelUser = "eevblog"
            };

            _yupRepository.ytChannels.Add(chann);


            _eventBus.RaiseEvent(EventOnBus.channelAdded, this, new EventBusArgs() { Item = chann });

            MessageBox.Show(zmienna??"nie ma");


        }

        /// <summary>
        /// Method responsible for publishing this View specific events
        /// </summary>
        public void PublishEvents()
        {
            _eventBus.PublishEvent(EventOnBus.channelAdded, channelAdded);
            _eventBus.PublishEvent(EventOnBus.channelRemoved, channelRemoved);
        }


        /// <summary>
        /// Method responsible for subscribing this View specific events
        /// </summary>
        public void SubscribeEvents()
        {
            
        }
    }
}
