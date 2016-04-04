using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public ObservableCollection<string> Testos { get; set; }



        public string test { get; set; }

        private List<YTVideo> xx = new List<YTVideo>();

        public VideosViewModel(IYtManager ytManager)
        {
            _ytManager = ytManager;
            Testos = new ObservableCollection<string>();


            //var uzytkownikId =  _ytManager.GetChannelIdForUserName("EEVblog");
            //var filmiki = ytManager.GetVideosFromChannel(uzytkownikId);
            //var cos = _ytManager.GetChannelStatistcs("EEVblog");


        }

        public async void LoadData()
        {

            //if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

           

            var muchos = await _ytManager.GetChannelIdAsync("EEVblog");
            var filmiki = await _ytManager.GetVideosFromChannelAsync(muchos);

            Testos.Add( muchos );


        }

    }

}
