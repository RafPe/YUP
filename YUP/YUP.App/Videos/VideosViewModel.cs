using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Google.Apis.YouTube.v3.Data;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Videos
{
    public class VideosViewModel : BindableBase
    {

        private IYtManager _ytManager;
        private bool _dataLoaded;

        public ObservableCollection<YTVideo> YtVideos { get; set; }

        public VideosViewModel(IYtManager ytManager)
        {
            _ytManager = ytManager;
            _dataLoaded = false;
            YtVideos = new ObservableCollection<YTVideo>();

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
