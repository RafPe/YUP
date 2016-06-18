using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Google.Apis.YouTube.v3.Data;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.Helpers;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vVideos
{
    public class VideosViewModel : BindableBase, IEventRegistrator
    {

        private IYtManager          _ytManager;
        private IEventBus           _eventBus;
        private IYupRepository      _yupRepository;

        private YTVideo             _selectedYtVideo;
        private YTChannel           _selectedYtChannel;


        public event EventBusHandler VideoIdChanged;

        //TODO:This needs to be changed or removed :/
        private bool _dataLoaded = false;

        private bool _isBusy;

        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        public string SearchBoxTerm { get; set; } = "";

        public RelayCommand SearchBoxCmd                    { get; private set; }

        //TODO: Remove this ....
        public RelayCommand                     test        { get; private set; }

        public ObservableCollection<string>     Categories  { get; set; } 

        public ObservableCollection<YTVideo>    YtVideos    { get; set; }
        public ObservableCollection<YTChannel>  YtChannels  { get; set; }


        public VideosViewModel(IYtManager ytManager, IEventBus eventBus, IYupRepository yupRepository)
        {
            _eventBus       = eventBus;
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;

            _dataLoaded = false;

            Categories = new ObservableCollection<string>();

            test        = new RelayCommand(onTest);
            YtVideos    = new ObservableCollection<YTVideo>();
            YtChannels  = new ObservableCollection<YTChannel>();
            YtChannels.AddRange(_yupRepository.GetAllYtChannels());

        }

        /// <summary>
        /// Event occuring when a channel is being removed from repository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="busargs"></param>
        private void evtChannelRemoved(object sender, EventBusArgs busargs)
        {
            var objEvt = (YTChannel) busargs.Item;

            YtChannels.Remove(objEvt);

        }

        /// <summary>
        /// Event occuring when a channel is being added to repository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="busargs"></param>
        private void evtChannelAdded(object sender, EventBusArgs busargs)
        {
            var objEvt = (YTChannel)busargs.Item;

            YtChannels.Add(objEvt);
        }


        public YTVideo SelectedYtVideo
        {
            get
            {
                return _selectedYtVideo;
            }
            set
            {
                _selectedYtVideo = value;
            }
        }

        public YTChannel SelectedYtChannel
        {
            get
            {
                return _selectedYtChannel;
            }
            set
            {
                _selectedYtChannel = value;
                LoadVideos(_selectedYtChannel.user,_selectedYtChannel.channelId);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = true;
            }
        }

        public void onTest()
        {
            _eventBus.RaiseEvent(EventOnBus.videoIdChanged, this, new EventBusArgs() { Item = SelectedYtVideo });
        }

        private async void LoadVideos(string userId, string channelId)
        {
            List<SearchResult> filmiki;

            var muchos  = await _ytManager.GetChannelIdForUserAsync(userId);

            if (!ReferenceEquals(muchos, null))
            {
                filmiki = await _ytManager.GetVideosFromChannelAsync(muchos);
            }
            else
            {
                filmiki = await _ytManager.GetVideosFromChannelAsync(channelId);
            }

            if (ReferenceEquals(filmiki, null)) return;



            // New colletion 
            YtVideos.Clear();

            List<YTVideo> xx = new List<YTVideo>();

            Random rnd = new Random();

            foreach (SearchResult searchResult in filmiki)
            {
                var tmpobj = new YTVideo()
                {
                    videoId            = searchResult.Id.VideoId,
                    channelId          = searchResult.Snippet.ChannelId,
                    publishDdate       = searchResult.Snippet.PublishedAt ?? new DateTime(1900, 1, 1),
                    title              = searchResult.Snippet.Title,
                    thumbnail          = searchResult.Snippet.Thumbnails.Default__?.Url ?? "empty",
                    description        = searchResult.Snippet.Description,
                    rating             = rnd.Next(0, 5),  // creates a number between 1 and 6
                    isAvailableOffline = true,
                    category           = "default"


            };

                YtVideos.Add(tmpobj);
            }
        }

        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            var test = _yupRepository.GetAllCategories();

            this.Categories.UpdateRange(test);

            if (_dataLoaded) return; // avoid all time loading of data

            _dataLoaded = true;       // Mark in advance ....

            //this.LoadVideos("mirekk36");

        }

        public void PublishEvents()
        {
            // Publish event when vidoId changes
            _eventBus.PublishEvent(EventOnBus.videoIdChanged, VideoIdChanged);

        }

        public void SubscribeEvents()
        {
            // Subscribe to channel changes 
            _eventBus.SubscribeEvent(EventOnBus.channelAdded, evtChannelAdded);
            _eventBus.SubscribeEvent(EventOnBus.channelRemoved, evtChannelRemoved);
        }
    }

}
