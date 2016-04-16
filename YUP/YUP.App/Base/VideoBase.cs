using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool                             isAvailableOffline  { get; set; }

        //public List<YTThumbnail>                thumbnails  { get; set; }
        public string[]                         tags                { get; set; }
    }
}
