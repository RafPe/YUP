using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vPlayer
{
    public class PlayerViewModel: BindableBase , IEventRegistrator
    {
        private IMediaPlayer        _mediaPlayer; // Hostowany obiekt flash player'a
        private IYupRepository      _yupRepository;
        private IYtManager          _ytManager;
        private IEventBus           _eventBus;
        private ChromiumWebBrowser  _webBrowser;

        public bool isBrowserReady;

        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        public string SearchBoxTerm { get; set; } = "";

        public RelayCommand SearchBoxCmd { get; private set; }
        public RelayCommand relayCmdPlay { get; private set; }
        public RelayCommand relayCmdStop { get; private set; }
        public RelayCommand relayCmdYupi { get; private set; }

        public ChromiumWebBrowser WebBrowser
        {
            get
            {
                return _webBrowser;
            }
            set
            {
                _webBrowser = value;
                if (_webBrowser != null) OnBrowserCreated();
            }
        }

        private void OnBrowserCreated()
        {
            //WebBrowser.ExecuteScriptAsync("XloadVideoById", "iZDQ0-Zbpt8");
            //WebBrowser.ExecuteScriptAsync("XplayThis");
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus)
        {
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;
            _eventBus       = eventbus;

            var sss = System.AppDomain.CurrentDomain.BaseDirectory;

            Uri xyx = new Uri(sss);



            _webBrowser = new ChromiumWebBrowser() {Address = $"{xyx.AbsolutePath}/YtPlayer.html" };
            _webBrowser.FrameLoadEnd += delegate(object sender, FrameLoadEndEventArgs args)
            {
                this.isBrowserReady = true;
                _webBrowser.ShowDevTools();
            };


            SearchBoxCmd = new RelayCommand(onSearchBoxCmd);
            relayCmdPlay = new RelayCommand(onRelayCmdPlay);
            relayCmdStop = new RelayCommand(onRelayCmdStop);
            relayCmdYupi = new RelayCommand(OnRelayCmdYupi);
        }


        //private async Task<int> GetYtPlayerCurrentTime()
        //{
        //    object co = 0;
        //    // Get Document Height
        //    var task = _webBrowser.EvaluateScriptAsync("XgetCurrentTime()", null);

        //    await task.ContinueWith(t =>
        //    {
        //        if (!t.IsFaulted)
        //        {
        //            var response = t.Result;
        //            co = response.Result;
        //        }
        //    }, TaskScheduler.FromCurrentSynchronizationContext());



        //}

        // Sync the async script execution
        public async Task<object> EvaluateScript(string script, object defaultValue, TimeSpan timeout)
        {
            object result = defaultValue;
            if (_webBrowser.IsBrowserInitialized && !_webBrowser.IsDisposed)
            {
                try
                {
                    var task = _webBrowser.EvaluateScriptAsync(script, timeout);
                    await task.ContinueWith(res => {
                        if (!res.IsFaulted)
                        {
                            var response = res.Result;
                            result = response.Success ? (response.Result ?? "null") : response.Message;
                        }
                    }).ConfigureAwait(false); // <-- This makes the task to synchronize on a different context
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            return result;
        }

        private void OnRelayCmdYupi()
        {
            object result = this.EvaluateScript("XgetCurrentTime()", 0, TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

            MessageBox.Show( result.ToString() );
        }

        private void onRelayCmdStop()
        {
           // WebBrowser.ExecuteScriptAsync("XstopThis");
            WebBrowser.EvaluateScriptAsync("XstopThis()", TimeSpan.FromSeconds(1));
        }

        private void onRelayCmdPlay()
        {
           // WebBrowser.ExecuteScriptAsync("XplayThis");

            WebBrowser.EvaluateScriptAsync("XplayThis()", TimeSpan.FromSeconds(1));
        }

        private void VideoIdChangedHandler(object sender, EventBusArgs busargs)
        {

            try
            {
                YTVideo ytVideo = (YTVideo)busargs.Item;

                _webBrowser.ExecuteScriptAsync("XloadVideoById", ytVideo.videoId);
            }
            catch (Exception ex)
            {
                var cosik = "";
            }



        }

        private void onSearchBoxCmd()
        {
           //var videoId2play =  _ytManager.GetVideoIdFromUrl(SearchBoxTerm);

           // if (videoId2play == null) return;

            //_eventBus.RaiseEvent(EventOnBus.videoIdChanged, this, new EventBusArgs() { Item = SearchBoxTerm });
            
        }

        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

        }



        public void PublishEvents()
        {
           
        }

        public void SubscribeEvents()
        {
            _eventBus.SubscribeEvent(EventOnBus.videoIdChanged, VideoIdChangedHandler);
        }
    }
}
