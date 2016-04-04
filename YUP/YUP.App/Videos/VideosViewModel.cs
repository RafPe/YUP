using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

        public List<string> xxx;

        public ObservableCollection<string> Testos { get; set; }



        public string test { get; set; }

        private List<YTVideo> xx = new List<YTVideo>();

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

            var muchos  = await _ytManager.GetChannelIdAsync("EEVblog");
            var filmiki = await _ytManager.GetVideosFromChannelAsync(muchos);


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

            MessageBox.Show("Loaded all :) ");




        }

    }

}
