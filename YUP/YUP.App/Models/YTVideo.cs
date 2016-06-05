using System;
using System.Collections.Generic;
using YUP.App.Base;

namespace YUP.App.Models
{
    /// <summary>
    /// Public class used to manage Youtube videos
    /// </summary>
    public class YTVideo : YtBase
    {
        public string                           videoId             { get; set; }
        public int                              duration            { get; set; } 
        public DateTime                         publishDdate        { get; set; }
        public bool                             isAvailableOffline  { get; set; }

        // Might be used in future?
        public Dictionary<string,string>        videoSource         { get; set; }


        public string[]                         tags                { get; set; }

        /// <summary>
        /// Constructor with default values
        /// </summary>
        public YTVideo()
        {
            isAvailableOffline  = false;
            category            = "default";
            rating              = 0;

        }
    }
}
