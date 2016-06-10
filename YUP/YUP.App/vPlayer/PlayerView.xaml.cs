using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CefSharp;
using YUP.App.Contracts;
using YUP.App.Events;
using YUP.App.MediaPlayers;
using YUP.App.Services;

namespace YUP.App.vPlayer
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private FlashAxControl  cc; // Hostowany obiekt flash player'a
        private IEventBus       _eventBus;


        public PlayerView()
        {
            _eventBus = ContainerHelper.GetService<IEventBus>();

            _eventBus.SubscribeEvent(EventOnBus.videoIdChanged, VideoIdChangedHandler);

            InitializeComponent();

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE HTML>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=unicode' />");
            sb.Append("<meta http-equiv='X-UA-Compatible' content='IE=9' /> ");
            sb.Append("</head>");
            sb.Append("  <body>");
            sb.Append("    <!-- 1. The <iframe> (and video player) will replace this <div> tag. -->");
            sb.Append("    <div id=\"player\"></div>");
            sb.Append("    <script>");
            sb.Append("      // 2. This code loads the IFrame Player API code asynchronously.");
            sb.Append("      var tag = document.createElement('script');");
            sb.Append("      tag.src = \"http://www.youtube.com/player_api\";");
            sb.Append("      var firstScriptTag = document.getElementsByTagName('script')[0];");
            sb.Append("      firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);");
            sb.Append("      // 3. This function creates an <iframe> (and YouTube player)");
            sb.Append("      //    after the API code downloads.");
            sb.Append("      var player;");
            sb.Append("      function onYouTubePlayerAPIReady() {");
            sb.Append("        player = new YT.Player('player', {");
            sb.Append("          height: '390',");
            sb.Append("          width: '640',");
            sb.Append("          videoId: 'u1zgFlCw8Aw',");
            sb.Append("          events: {");
            sb.Append("            'onReady': onPlayerReady");
            sb.Append("          }");
            sb.Append("        });");
            sb.Append("      }");
            sb.Append("      // 4. The API will call this function when the video player is ready.");
            sb.Append("      function onPlayerReady(event) {");
            sb.Append("        event.target.playVideo();");
            sb.Append("      }");
            sb.Append("      // 5. The API calls this function when the player's state changes.");
            sb.Append("      //    The function indicates that when playing a video (state=1),");
            sb.Append("      //    the player should play for six seconds and then stop.");
            sb.Append("      var done = false;");
            sb.Append("      function onPlayerStateChange(event) {");
            sb.Append("        if (event.data == YT.PlayerState.PLAYING && !done) {");
            sb.Append("          setTimeout(stopVideo, 6000);");
            sb.Append("          done = true;");
            sb.Append("        }");
            sb.Append("      }");
            sb.Append("      function stopVideo() {");
            sb.Append("        player.stopVideo();");
            sb.Append("      }");
            sb.Append("     function playVideo() {");
            sb.Append("        player.playVideo();");
            sb.Append("      }");
            sb.Append("     function pauseVideo() {");
            sb.Append("        player.pauseVideo();");
            sb.Append("      }");
            sb.Append("     function gotoTime(time){");
            sb.Append("        player.seekTo(time,false);");
            sb.Append("     }");
            sb.Append("    function getCurrentTime() {");
            sb.Append("            document.getElementById('currentTime').innerHTML = player.getCurrentTime();");
            sb.Append("        }");
            sb.Append("        function getWidth() {");
            sb.Append("        }");
            sb.Append("        function getHeight() {");
            sb.Append("        }");
            sb.Append("    </script>");
            sb.Append("    <div>");
            sb.Append("        <button type=\"button\" onClick=\"playVideo()\">Play</button>");
            sb.Append("        <button type=\"button\" onClick=\"pauseVideo()\">Pause</button>");
            sb.Append("        <button type=\"button\" onClick=\"stopVideo()\">Stop</button>");
            sb.Append("        <button type=\"button\" onClick=\"gotoTime(200)\">GoTo</button>");
            sb.Append("        <button type=\"button\" onClick=\"getTime()\">Query Time</button>");
            sb.Append("    </div>");
            sb.Append("    <div>");
            sb.Append("     <label>current time:</label>");
            sb.Append("     <label id=\"currentTime\"></label>");
            sb.Append("     </div>");
            sb.Append("  </body>");
            sb.Append("</html>");


            WebBrowser.WebBrowser.LoadString(sb.ToString(),"http:/me.com".ToString());

            //webBrowser.CurrentUrl = @"c:\temp\testme.html";

            //webBrowser.Loaded += delegate(object sender, RoutedEventArgs args)
            //{
            //    HideScriptErrors((WebBrowser)sender,true);
            //};
            //webBrowser.LoadCompleted += WebBrowserOnLoadCompleted;


            //this.webBrowser.ShowYouTubeVideo("L8bE5-g8VC0");

            //Uri uri = new Uri(@"C:\temp\testme.html",UriKind.Absolute);
            //Stream source = Application.GetContentStream(uri).Stream;
            //webBrowser.NavigateToStream(source);
            //webBrowser.Navigate(uri);
            //var eee = "";



        }

        private void WebBrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            //    
            //webBrowser.Source = new Uri(string.Format("javascript: {0}();", "stopVideo"));

            //  if (IsLoaded) webBrowser.InvokeScript("stopVideo");


        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private void VideoIdChangedHandler(object sender, EventBusArgs busargs)
        {
            //var test = (string) busargs.Item;

            //cc.mediaLoadVideo(test);

            int tmp = 0;

            

           // webBrowser.InvokeScript("stopVideo");


        }



        private void PlayerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var host = new WindowsFormsHost(); // Interop z Windows Forms

            //cc = (FlashAxControl)ContainerHelper.GetService<IMediaPlayer>("youtube");

            //host.Child = cc;
            //int Height = (int) player_youtube.ActualHeight;
            //int Width = (int) player_youtube.ActualWidth;

            //cc.Height = 500;
            //cc.Width = 1020;

            //cc.mediaSetPlayerSize(400, 200);


            //player_youtube.Children.Add(host);


        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void WebBrowser_OnLoaded(object sender, RoutedEventArgs e)
        {
     //       webBrowser.InvokeScript("stopVideo");
        }

        private void WebBrowser_OnLoadCompleted(object sender, NavigationEventArgs e)
        {

            //var browser = sender as WebBrowser;

            //dynamic document = browser.Document;

            //document.myfunc();

            //var browser = sender as WebBrowser;

            //if (browser == null || browser.Document == null)
            //    return;

            //dynamic document = browser.Document;

            //if (document.readyState != "complete")
            //    return;

            //dynamic script = document.createElement("script");
            //script.type = @"text/javascript";
            //script.text = @"window.onerror = function(){player.loadVideoById('3PEj8_0Q9Qo');}";
            //document.head.appendChild(script);


            //webBrowser.InvokeScript("stopVideo");
        }
    }


    public static class WebBrowserExtensions
    {
        private static string GetYouTubeVideoPlayerHTML(string videoCode)
        {
            //var sb = new StringBuilder();

            const string YOUTUBE_URL = @"http://www.youtube.com/v/";

            //sb.Append("<html>");
            //sb.Append("    <head>");
            //sb.Append("        <meta name=\"viewport\" content=\"width=device-width; height=device-height;\">");
            //sb.Append("    </head>");
            //sb.Append("    <body marginheight=\"0\" marginwidth=\"0\" leftmargin=\"0\" topmargin=\"0\" style=\"overflow-y: hidden\">");
            //sb.Append("        <object width=\"100%\" height=\"100%\">");
            //sb.Append("            <param name=\"movie\" value=\"" + YOUTUBE_URL + videoCode + "?version=3&amp;rel=0\" />");
            //sb.Append("            <param name=\"allowFullScreen\" value=\"true\" />");
            //sb.Append("            <param name=\"allowscriptaccess\" value=\"always\" />");
            //sb.Append("            <embed src=\"" + YOUTUBE_URL + videoCode + "?version=3&amp;rel=0\" type=\"application/x-shockwave-flash\"");
            //sb.Append("                   width=\"100%\" height=\"100%\" allowscriptaccess=\"always\" allowfullscreen=\"true\" />");
            //sb.Append("        </object>");
            //sb.Append("    </body>");
            //sb.Append("</html>");


            // BuildMyString.com generated code. Please enjoy your string responsibly.
            // BuildMyString.com generated code. Please enjoy your string responsibly.

            // BuildMyString.com generated code. Please enjoy your string responsibly.

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE HTML>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=unicode' />");
            sb.Append("<meta http-equiv='X-UA-Compatible' content='IE=9' /> ");
            sb.Append("</head>");
            sb.Append("  <body>");
            sb.Append("    <!-- 1. The <iframe> (and video player) will replace this <div> tag. -->");
            sb.Append("    <div id=\"player\"></div>");
            sb.Append("    <script>");
            sb.Append("      // 2. This code loads the IFrame Player API code asynchronously.");
            sb.Append("      var tag = document.createElement('script');");
            sb.Append("      tag.src = \"http://www.youtube.com/player_api\";");
            sb.Append("      var firstScriptTag = document.getElementsByTagName('script')[0];");
            sb.Append("      firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);");
            sb.Append("      // 3. This function creates an <iframe> (and YouTube player)");
            sb.Append("      //    after the API code downloads.");
            sb.Append("      var player;");
            sb.Append("      function onYouTubePlayerAPIReady() {");
            sb.Append("        player = new YT.Player('player', {");
            sb.Append("          height: '390',");
            sb.Append("          width: '640',");
            sb.Append("          videoId: 'u1zgFlCw8Aw',");
            sb.Append("          events: {");
            sb.Append("            'onReady': onPlayerReady");
            sb.Append("          }");
            sb.Append("        });");
            sb.Append("      }");
            sb.Append("      // 4. The API will call this function when the video player is ready.");
            sb.Append("      function onPlayerReady(event) {");
            sb.Append("        event.target.playVideo();");
            sb.Append("      }");
            sb.Append("      // 5. The API calls this function when the player's state changes.");
            sb.Append("      //    The function indicates that when playing a video (state=1),");
            sb.Append("      //    the player should play for six seconds and then stop.");
            sb.Append("      var done = false;");
            sb.Append("      function onPlayerStateChange(event) {");
            sb.Append("        if (event.data == YT.PlayerState.PLAYING && !done) {");
            sb.Append("          setTimeout(stopVideo, 6000);");
            sb.Append("          done = true;");
            sb.Append("        }");
            sb.Append("      }");
            sb.Append("      function stopVideo() {");
            sb.Append("        player.stopVideo();");
            sb.Append("      }");
            sb.Append("     function playVideo() {");
            sb.Append("        player.playVideo();");
            sb.Append("      }");
            sb.Append("     function pauseVideo() {");
            sb.Append("        player.pauseVideo();");
            sb.Append("      }");
            sb.Append("     function gotoTime(time){");
            sb.Append("        player.seekTo(time,false);");
            sb.Append("     }");
            sb.Append("    function getCurrentTime() {");
            sb.Append("            document.getElementById('currentTime').innerHTML = player.getCurrentTime();");
            sb.Append("        }");
            sb.Append("        function getWidth() {");
            sb.Append("        }");
            sb.Append("        function getHeight() {");
            sb.Append("        }");
            sb.Append("    </script>");
            sb.Append("    <div>");
            sb.Append("        <button type=\"button\" onClick=\"playVideo()\">Play</button>");
            sb.Append("        <button type=\"button\" onClick=\"pauseVideo()\">Pause</button>");
            sb.Append("        <button type=\"button\" onClick=\"stopVideo()\">Stop</button>");
            sb.Append("        <button type=\"button\" onClick=\"gotoTime(200)\">GoTo</button>");
            sb.Append("        <button type=\"button\" onClick=\"getTime()\">Query Time</button>");
            sb.Append("    </div>");
            sb.Append("    <div>");
            sb.Append("     <label>current time:</label>");
            sb.Append("     <label id=\"currentTime\"></label>");
            sb.Append("     </div>");
            sb.Append("  </body>");
            sb.Append("</html>");

//            indows Registry Editor Version 5.00
//[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION] "contoso.exe" = dword:00002328




            return sb.ToString();
        }

        public static void ShowYouTubeVideo(this WebBrowser webBrowser, string videoCode)
        {
            if (webBrowser == null) throw new ArgumentNullException("webBrowser");

            webBrowser.NavigateToString(GetYouTubeVideoPlayerHTML(videoCode));
        }
    }
}


//var yyy = ContainerHelper.GetService<IMediaPlayer>("youtube");
//yyy.mediaLoadVideo("b9FC9fAlfQE");