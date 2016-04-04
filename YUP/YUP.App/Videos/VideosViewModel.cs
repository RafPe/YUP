using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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



    public class SmartCollection<T> : ObservableCollection<T> {
    public SmartCollection()
        : base() {
    }

    public SmartCollection(IEnumerable<T> collection)
        : base(collection) {
    }

    public SmartCollection(List<T> list)
        : base(list) {
    }

    public void AddRange(IEnumerable<T> range) {
        foreach (var item in range) {
            Items.Add(item);
        }

        this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Reset(IEnumerable<T> range) {
        this.Items.Clear();

        AddRange(range);
    }
}

}
