using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CefSharp;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.MediaPlayers;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vPlayer
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class PlayerView : UserControl , IEventRegistrator
    {
        private FlashAxControl  cc; // Hostowany obiekt flash player'a
        private IEventBus       _eventBus;

        private String sources;

        public PlayerView()
        {
            _eventBus = ContainerHelper.GetService<IEventBus>();

            InitializeComponent();

        }

        private void WebBrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {

        }


        private void VideoIdChangedHandler(object sender, EventBusArgs busargs)
        {

            try
            {
                YTVideo ytVideo = (YTVideo)busargs.Item;

                //xxx.ExecuteScriptAsync("XloadVideoById", ytVideo.videoId);
            }
            catch (Exception ex)
            {
                var cosik = "";
            }



        }



        private void Xxx_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void Xxx_OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
           // xxx.ShowDevTools();


        }



        private async void PlayerGetcurrentTime()
        {
            //object co = "";
            //// Get Document Height
            //var task = xxx.EvaluateScriptAsync("XgetCurrentTime()", null);

            //await task.ContinueWith(t =>
            //{
            //    if (!t.IsFaulted)
            //    {
            //        var response = t.Result;
            //        co = response.Result;
            //    }
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //MessageBox.Show(co.ToString());

        }


        public void PublishEvents()
        {
                
        }

        public void SubscribeEvents()
        {
            _eventBus.SubscribeEvent(EventOnBus.videoIdChanged, VideoIdChangedHandler);
        }

        private void BtnPlay_OnClick(object sender, RoutedEventArgs e)
        {
            //xxx.ExecuteScriptAsync("XloadVideoById", "iZDQ0-Zbpt8");
            //xxx.ExecuteScriptAsync("XplayThis");
        }

        private void BtnStop_OnClick(object sender, RoutedEventArgs e)
        {
            //xxx.ExecuteScriptAsync("XstopThis");
        }

        private void BtnPause_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCurrentTime_OnClick(object sender, RoutedEventArgs e)
        {
            PlayerGetcurrentTime();
        }
    }
}

