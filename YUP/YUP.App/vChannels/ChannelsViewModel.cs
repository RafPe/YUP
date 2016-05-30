using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private IYupRepository _yupRepository;
        private YTChannel _selectedYtChannel;
        private IYtManager _ytManager;
        private IEventBus _eventBus;


        public ObservableCollection<string> test { get; set; }
        public ObservableCollection<YTChannel> YtChannels { get; set; }


        public event EventBusHandler channelAdded;
        public event EventBusHandler channelRemoved;

        public YTChannel SelectedYtChannel
        {
            get { return _selectedYtChannel; }
            set
            {
                _selectedYtChannel = value;
                SearchBoxTerm = value.channelId ?? "";
            }
        }


        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        public string SearchBoxTerm { get; set; } = "";

        public RelayCommand XYZ { get; private set; }
        public RelayCommand cmdRemoveChannel { get; private set; }
        public RelayCommand SearchBoxCmd { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="yupRepository"></param>
        /// <param name="ytManager"></param>
        /// <param name="eventbus"></param>
        public ChannelsViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus)
        {
            _yupRepository = yupRepository;
            _ytManager = ytManager;
            _eventBus = eventbus;

            YtChannels = new ObservableCollection<YTChannel>();

            XYZ = new RelayCommand(testme);
            SearchBoxCmd = new RelayCommand(klinkal);
            cmdRemoveChannel = new RelayCommand(onCmdRemoveChannel);

        }

        private void testme()
        {
            var cos = "";
        }

        public void onCmdRemoveChannel()
        {
            YtChannels.Remove(SelectedYtChannel);
        }

        public async void klinkal()
        {

            if (string.IsNullOrWhiteSpace(SearchBoxTerm)) return;

            string tmpChannelID;

            // First we check if we got details by username
            tmpChannelID = await _ytManager.GetChannelIdForUserAsync(SearchBoxTerm);

            if (tmpChannelID == null) tmpChannelID = _ytManager.GetChannelIdFromUrl(SearchBoxTerm);

            if (tmpChannelID == null) return;

            var x = _ytManager.GetChannelStatistcsByChannelId(tmpChannelID);

            if(x==null)  x = _ytManager.GetChannelStatistcsByUser(SearchBoxTerm);

            var chann = new YTChannel()
            {
                description =
                    $"{x.Snippet.Description.Substring(0, x.Snippet.Description.Length < 100 ? x.Snippet.Description.Length : 100).Trim()} ... <read more>",
                thumbnail = x.Snippet.Thumbnails.High.Url,
                channelId = x.Id,
                channelUser = SearchBoxTerm
            };

            _yupRepository.ytChannels.Add(chann);
            _yupRepository.SaveRepository();
            YtChannels.Add(chann);


            _eventBus.RaiseEvent(EventOnBus.channelAdded, this, new EventBusArgs() {Item = chann});

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

        /// <summary>
        /// Asyncronous method used 
        /// </summary>
        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            YtChannels.AddRange(_yupRepository.ytChannels);

        }
    }
}
