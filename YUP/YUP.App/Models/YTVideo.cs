using System;
using System.Collections.Generic;
using YUP.App.Base;

namespace YUP.App.Models
{
    /// <summary>
    /// Public class used to manage Youtube videos
    /// </summary>
    public class YTVideo : VideoBase
    {
        public string                           channelId           { get; set; }

        //public List<YTThumbnail>                thumbnails  { get; set; }
        public string[]                         tags                { get; set; }
    }
}
