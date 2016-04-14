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

        private IYtManager  _ytManager;
        private IEventBus   _eventBus;

        public event EventBusHandler VideoIdChangedHandler;

        //TODO:This needs to be changed or removed :/
        private bool _dataLoaded;
        public RelayCommand test { get; private set; }

        public ObservableCollection<YTVideo> YtVideos { get; set; }

        public VideosViewModel(IYtManager ytManager, IEventBus eventBus)
        {
            _eventBus = eventBus;
            

            _ytManager = ytManager;
            _dataLoaded = false;
            test = new RelayCommand(onTest);
            YtVideos = new ObservableCollection<YTVideo>();

            _eventBus.PublishEvent("VideoIdChanged", VideoIdChangedHandler);

        }

        private YTVideo _selectedYtVideo;
        public YTVideo SelectedYtVideo
        {
            get
            {
                return _selectedYtVideo;
            }
            set { _selectedYtVideo = value; }
        }


        public void onTest()
        {
            _eventBus.RaiseEvent("VideoIdChanged", this, new EventBusArgs() { Item = SelectedYtVideo.videoId });
        }

        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;


            if (_dataLoaded) return; // avoid all time loading of data

            _dataLoaded = true;       // Mark in advance ....

            var muchos  = await _ytManager.GetChannelIdAsync("eevblog");
            var filmiki = await _ytManager.GetVideosFromChannelAsync(muchos);

            List<YTVideo> xx = new List<YTVideo>();


            foreach (SearchResult searchResult in filmiki)
            {
                var tmpobj = new YTVideo()
                {
                    videoId         = searchResult.Id.VideoId,
                    channelId       = searchResult.Snippet.ChannelId,
                    publishDate     = searchResult.Snippet.PublishedAt ?? new DateTime(1900, 1, 1),
                    title           = searchResult.Snippet.Title,
                    thumbnail       = searchResult.Snippet.Thumbnails.Default__?.Url ?? "empty"
                };

                YtVideos.Add(tmpobj);
            }

            //YtVideos.Add(xx);

            MessageBox.Show("Loaded all :) ");




        }

    }

}
