using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YUP.Core.Enums;
using YUP.Core.Events;
using YUP.Core.Services;

namespace YUP.Player.Youtube
{
    using System.Text.RegularExpressions;
    using System.Xml;

    public partial class FlashAxControl : UserControl,IPlayer
    {
        #region Public Props

        public string   description     { get; } = "Plugin providing youtube player functionality";
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
        public string currentlyPlayingVideoId => _currentlyPlayingVideoId;
        private string _currentlyPlayingVideoId { get; set; }

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
            _currentlyPlayingVideoId = "BboXNHDjhAM";
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
            _currentlyPlayingVideoId    = currentlyPlayingVideoId;
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
        private void PlayerStateTrigger(Player_VideoStatus status,string videoId)
        {
            PlayerStateChanged?.Invoke(null, new PlayerStateChangedArgs { PlayerVideoStatus = status, VideoId = videoId});
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
            _currentlyPlayingVideoId = videoId;

            YTplayer_CallFlash(IsVideoIdValidUrl(videoId)
                ? $"loadVideoById({videoId},0,{videoquality})"
                : $"loadVideoByUrl({videoId},0,{videoquality})");
        }

        public void mediaLoadVideo(string videoId, int startFrom, string videoquality = "default")
        {
            _currentlyPlayingVideoId = videoId;

            YTplayer_CallFlash(IsVideoIdValidUrl(videoId)
                ? $"loadVideoById({videoId},{startFrom},{videoquality})"
                : $"loadVideoByUrl({videoId},{startFrom},{videoquality})");
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
        /// This method checks if we have valid URL for youtube
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        private bool IsVideoIdValidUrl(string videoid)
        {

            var r = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*");
            
            var match = r.Match(videoid);

            if (match.Success)
            {
                if (match.Groups["Domain"].Value.ToLower() == "youtu.be" ||
                    match.Groups["Domain"].Value.ToLower() == "youtube.com")
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

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
                case "document.location.href.toString": YTplayer.SetReturnValue("<string>http://www.youtube.com/watch?v=" + currentlyPlayingVideoId + "</string>"); break;
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
                    PlayerStateTrigger(Player_VideoStatus.notstarted, currentlyPlayingVideoId); break; //not started yet
                case 1: playState = true;  PlayerStateTrigger(Player_VideoStatus.playing, currentlyPlayingVideoId); break; //playing
                case 2: playState = false; PlayerStateTrigger(Player_VideoStatus.paused, currentlyPlayingVideoId); break; //paused
                case 3:
                    playState = false; PlayerStateTrigger(Player_VideoStatus.buffering, currentlyPlayingVideoId); break; //buffering
                case 0:
                    playState = false;
                    //if (!loopFile) mediaNext(); else YTplayer_CallFlash("seekTo(0)"); 
                    PlayerStateTrigger(Player_VideoStatus.ended, currentlyPlayingVideoId);
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
