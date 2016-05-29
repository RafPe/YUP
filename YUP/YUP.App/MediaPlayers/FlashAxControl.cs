using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using YUP.App.Events;
using YUP.App.Services;

namespace YUP.App.MediaPlayers
{
    public partial class FlashAxControl : UserControl,IMediaPlayer
    {
        #region Public Props

        public string   description     { get; } = "Player providing youtube player functionality";
        public string   author          { get; } = "Created by RafPe";
        public string   contact         { get; } = "https://github.com/RafPe/YUP";
        public string   version         { get; } = "1.0";
        public bool     isEnabled       { get; set; }

        public bool     playState       = false;
        public bool     YTState         = false;

        /// <summary>
        /// Property defining our video provider
        /// </summary>
        public string video_provider { get; } = "youtube";

        /// <summary>
        /// Property defining currently being played videoId
        /// </summary>
        public string currentVideoId => _currentVideoId;
        private string _currentVideoId { get; set; }

        /// <summary>
        /// Event which is called on every state change of our player
        /// </summary>
        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        /// <summary>
        /// Player width
        /// </summary>
        public new int Width
        {
            get { return YTplayer.Width; }
            set { YTplayer.Width = value; }
        }

        /// <summary>
        /// Player height
        /// </summary>
        public new int Height
        {
            get { return YTplayer.Height; }
            set { YTplayer.Height = value; }
        }

        #endregion

        #region ctors
        /// <summary>
        /// Default constructor
        /// </summary>
        public FlashAxControl()
        {
            _currentVideoId = "BboXNHDjhAM";
            InitializeComponent();
        }

        /// <summary>
        /// Extended properties constructor
        /// </summary>
        /// <param name="h">target height for player</param>
        /// <param name="w">target width for player</param>
        /// <param name="videoId">target videoId</param>
        /// <param name="currentlyPlayingVideoId"></param>
        /// <param name="quality">desired video quality</param>
        public FlashAxControl(int h, int w ,string videoId , string currentlyPlayingVideoId, string quality="default")
        {
            _currentVideoId             = currentlyPlayingVideoId;
            Width                       = w;
            Height                      = h;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event triggered when our player changes state
        /// </summary>
        /// <param name="status"> new status of our player</param>
        /// <param name="videoId"> our videoId which is being used</param>
        private void PlayerStateTrigger(MediaPlaybackState status,string videoId)
        {
            PlayerStateChanged?.Invoke(null, new PlayerStateChangedArgs { playbackState = status, videoId = videoId});
        }

        #endregion

        #region Player Methods

        public void mediaPlay()
        {
            YTplayer_CallFlash("playVideo()");
            playState = true;

        }

        public void mediaPause()
        {
            YTplayer_CallFlash("pauseVideo()");
            playState = false;

        }

        public void mediaLoadVideo(string videoId, string videoquality = "default")
        {

            if (IsVideoIdValidUrl(videoId))
            {
                 videoId = GetVideoIdFromUrl(videoId);
            }

            // No Id and no URL - we bail out
            if (videoId == null) return;
            

            _currentVideoId = videoId;

            var videoURL = $"https://www.youtube.com/v/{videoId}?version=3";

            YTplayer_CallFlash($"loadVideoByUrl({videoURL},0,{videoquality})");
        }

        public void mediaLoadVideo(string videoId, int startFrom, string videoquality = "default")
        {
            if (IsVideoIdValidUrl(videoId))
            {
                videoId = GetVideoIdFromUrl(videoId);
            }

            // No Id and no URL - we bail out
            if (videoId == null) return;


            _currentVideoId = videoId;

            var videoURL = $"https://www.youtube.com/v/{videoId}?version=3";

            YTplayer_CallFlash($"loadVideoByUrl({videoURL},{startFrom},{videoquality})");

        }

        public void mediaPlayPause()
        {
            if (playState == true)
            {
                mediaPause();
            }
            else
            {
                mediaPlay();
            }
        }

        public int mediaGetCurrentTime()
        {

            return int.Parse(Regex.Match(YTplayer_CallFlash("getCurrentTime()"), @"\d+").Value);
        }

        public void mediaSetPlayerSize(int w, int h)
        {
            YTplayer_CallFlash($"setSize({w.ToString()},{h.ToString()})");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets video Id from given URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetVideoIdFromUrl(string url)
        {
            var r = new Regex(@"^.*(?:(?:youtu\.be\/|v\/|vi\/|u\/\w\/|embed\/)|(?:(?:watch)?\?v(?:i)?=|\&v(?:i)?=))(?<videoId>[^#\&\?]*).*");

            var match = r.Match(url);

            if (match.Success && !String.IsNullOrEmpty(match.Groups["videoId"].Value.ToLower()))
            {
                return match.Groups["videoId"].Value;
            }

            return null;
        }

        /// <summary>
        /// This method checks if we have valid URL
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        private bool IsVideoIdValidUrl(string videoid)
        {

            var r = new Regex(@"^http(s)?.*");

            var match = r.Match(videoid);

            if (match.Success) return true;

            return false;

        } 

        /// <summary>
        /// Method used to call flash functions
        /// </summary>
        /// <param name="ytFunction"></param>
        /// <returns></returns>
        private string YTplayer_CallFlash(string ytFunction)
        {
            string flashXMLrequest = "";
            string response = "";
            string flashFunction = "";
            List<string> flashFunctionArgs = new List<string>();

            Regex func2xml = new Regex(@"([a-z][a-z0-9]*)(\(([^)]*)\))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match fmatch = func2xml.Match(ytFunction);

            if (fmatch.Captures.Count != 1)
            {
                //TODO Error control
                //Console.Write("bad function request string");
                return "";
            }

            flashFunction = fmatch.Groups[1].Value.ToString();
            flashXMLrequest = "<invoke name=\"" + flashFunction + "\" returntype=\"xml\">";
            if (fmatch.Groups[3].Value.Length > 0)
            {
                flashFunctionArgs = parseDelimitedString(fmatch.Groups[3].Value);
                if (flashFunctionArgs.Count > 0)
                {
                    flashXMLrequest += "<arguments><string>";
                    flashXMLrequest += string.Join("</string><string>", flashFunctionArgs);
                    flashXMLrequest += "</string></arguments>";
                }
            }
            flashXMLrequest += "</invoke>";

            try
            {
                //TODO ERROR CONTROL
                response = YTplayer.CallFunction(flashXMLrequest);
            }
            catch
            {

            }

            return response;
        }

        /// <summary>
        /// Object callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YTplayer_FlashCall(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FlashCallEvent e)
        {
            // message is in xml format so we need to parse it
            XmlDocument document = new XmlDocument();
            document.LoadXml(e.request);
            // get attributes to see which command flash is trying to call
            XmlAttributeCollection attributes = document.FirstChild.Attributes;
            String command = attributes.Item(0).InnerText;
            // get parameters
            XmlNodeList list = document.GetElementsByTagName("arguments");

            List<string> listS = (from XmlNode l in list select l.InnerText).ToList();

            // Interpret command
            switch (command)
            {
                case "onYouTubePlayerReady": YTready(listS[0]); break;
                case "YTStateChange": YTStateChange(listS[0]); break;
                case "YTError": YTStateError(listS[0]); break;
                case "document.location.href.toString": YTplayer.SetReturnValue("<string>http://www.youtube.com/watch?v=" + currentVideoId + "</string>"); break;
                default: Console.Write("YTplayer_FlashCall: (unknownCommand)\r\n"); break;
            }
        }

        private void YTStateError(string error)
        {
            //TODO handle error state from YT
        }

        /// <summary>
        /// Method handling our video player states
        /// </summary>
        /// <param name="YTplayState"></param>
        private void YTStateChange(string YTplayState)
        {
            switch (int.Parse(YTplayState))
            {
                case -1: playState = false;
                    PlayerStateTrigger(MediaPlaybackState.notstarted, currentVideoId); break; //not started yet
                case 1: playState = true;  PlayerStateTrigger(MediaPlaybackState.playing, currentVideoId); break; //playing
                case 2: playState = false; PlayerStateTrigger(MediaPlaybackState.paused, currentVideoId); break; //paused
                case 3:
                    playState = false; PlayerStateTrigger(MediaPlaybackState.buffering, currentVideoId); break; //buffering
                case 0:
                    playState = false;
                    //if (!loopFile) mediaNext(); else YTplayer_CallFlash("seekTo(0)"); 
                    PlayerStateTrigger(MediaPlaybackState.ended, currentVideoId);
                    break; //ended
            }
        }

        private void YTready(string playerID)
        {
            YTState = true;

            //start eventHandlers
            YTplayer_CallFlash("addEventListener(\"onStateChange\",\"YTStateChange\")");
            YTplayer_CallFlash("addEventListener(\"onError\",\"YTError\")");

        }

        private void YTplayer_FSCommand(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEvent e)
        {
            Console.Write("YTplayer_FSCommand: " + e.command.ToString() + "(" + e.args.ToString() + ")" + "\r\n");
        }

        private static List<string> parseDelimitedString(string arguments, char delim = ',')
        {
            bool inQuotes = false;
            bool inNonQuotes = false;
            int whiteSpaceCount = 0;

            List<string> strings = new List<string>();

            StringBuilder sb = new StringBuilder();
            foreach (char c in arguments)
            {
                if (c == '\'' || c == '"')
                {
                    if (!inQuotes)
                        inQuotes = true;
                    else
                        inQuotes = false;

                    whiteSpaceCount = 0;
                }
                else if (c == delim)
                {
                    if (!inQuotes)
                    {
                        if (whiteSpaceCount > 0 && inQuotes)
                        {
                            sb.Remove(sb.Length - whiteSpaceCount, whiteSpaceCount);
                            inNonQuotes = false;
                        }
                        strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());
                        sb.Remove(0, sb.Length);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    whiteSpaceCount = 0;
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (inNonQuotes || inQuotes)
                    {
                        sb.Append(c);
                        whiteSpaceCount++;
                    }
                }
                else
                {
                    if (!inQuotes) inNonQuotes = true;
                    sb.Append(c);
                    whiteSpaceCount = 0;
                }
            }
            strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());


            return strings;
        }

        #endregion


    }
}
