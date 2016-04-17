using System;
using System.Collections.Generic;

namespace YUP.App.Base
{
    public class VideoBase
    {
        public string                           title               { get; set; }   
        public string                           description         { get; set; }
        public string                           videoId             { get; set; }


        public int                              duration            { get; set; } 
        public DateTime                         publishDdate        { get; set; }
        public string                           thumbnail           { get; set; }
        public Dictionary<string,string>        videoSource         { get; set; }

        public bool                             isWatched           { get; set; }
        public bool                             isFavorite          { get; set; }
        public bool                             isHidden            { get; set; }
        public bool                             isSelected          { get; set; } //Used for multiple items editing 
        public bool                             isAvailableOffline  { get; set; }

        //public List<YTThumbnail>                thumbnails  { get; set; }
        public string[]                         tags                { get; set; }
    }
}
