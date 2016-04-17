using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Google.Apis.YouTube.v3.Data;
using YUP.App.Contracts;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Videos
{
    public class VideosViewModel : BindableBase
    {

        private IYtManager          _ytManager;
        private IEventBus           _eventBus;
        private IYupRepository      _yupRepository;

        private YTVideo             _selectedYtVideo;
        private YTChannel           _selectedYtChannel;


        public event EventBusHandler VideoIdChangedHandler;

        //TODO:This needs to be changed or removed :/
        private bool _dataLoaded;
        public RelayCommand test { get; private set; }

        public ObservableCollection<YTVideo>    YtVideos   { get; set; }
        public ObservableCollection<YTChannel>  YtChannels { get; set; }

        public VideosViewModel(IYtManager ytManager, IEventBus eventBus, IYupRepository yupRepository)
        {
            _eventBus       = eventBus;
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;

            _dataLoaded = false;

            test        = new RelayCommand(onTest);
            YtVideos    = new ObservableCollection<YTVideo>();
            YtChannels  = new ObservableCollection<YTChannel>();

            _eventBus.PublishEvent("VideoIdChanged", VideoIdChangedHandler);

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
                LoadVideos(_selectedYtChannel.channelUser);
            }
        }

        public void onTest()
        {
            _eventBus.RaiseEvent("VideoIdChanged", this, new EventBusArgs() { Item = SelectedYtVideo.videoId });
        }

        private async void LoadVideos(string userId)
        {

            var muchos = await _ytManager.GetChannelIdAsync(userId);
            var filmiki = await _ytManager.GetVideosFromChannelAsync(muchos);

            // New colletion 
            YtVideos.Clear();

            List<YTVideo> xx = new List<YTVideo>();


            foreach (SearchResult searchResult in filmiki)
            {
                var tmpobj = new YTVideo()
                {
                    videoId = searchResult.Id.VideoId,
                    channelId = searchResult.Snippet.ChannelId,
                    publishDdate = searchResult.Snippet.PublishedAt ?? new DateTime(1900, 1, 1),
                    title = searchResult.Snippet.Title,
                    thumbnail = searchResult.Snippet.Thumbnails.Default__?.Url ?? "empty"
                };

                YtVideos.Add(tmpobj);
            }
        }

        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;


            if (_dataLoaded) return; // avoid all time loading of data

            _dataLoaded = true;       // Mark in advance ....

            var lista = new List<YTChannel>();
            lista.Add(new YTChannel()
            {
                channelId = "Electronics vBlog",
                channelFriendlyName = "Electronics blog",
                channelUser = "eevblog"
            });
            lista.Add(new YTChannel()
            {
                channelId = "666",
                channelFriendlyName = "Electronics blog",
                channelUser = "mirekk36"
            });

            YtChannels.Add(lista[0]);
            YtChannels.Add(lista[1]);






            _yupRepository.ytChannels = lista;




            _yupRepository.SaveRepository();





        }

    }

}
