using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Videos
{
    public class VideosViewModel : BindableBase
    {
        private IYtManager _ytManager;

        public ObservableCollection<YTVideo> YtVideos { get; set; }

        public List<string> xxx;

        public string test { get; set; }

        private List<YTVideo> xx = new List<YTVideo>();

        public VideosViewModel(IYtManager ytManager)
        {
            _ytManager = ytManager;



            xx.Add(new YTVideo()
            {
                channelId = "dddd",
                description = "desc",
                duration = "222",
                isFavorite = false,
                isHidden = false,
                isWatched = false,
                publishDate = "121",
                tags = new[]
    {
                    "aaa",
                    "bbb"
                },
                title = "mytitle",
                videoId = "someId"
            });
            YtVideos = new ObservableCollection<YTVideo>(xx);

        }

        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;



            xxx = new List<string>()
            {
                "e",
                "123",
                "213123"
            };

            test = "kurwa-mac;";

        }

    }

}
