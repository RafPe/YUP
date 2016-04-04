using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Models
{
    public class YTVideo
    {
        public string                   title       { get; set; }
        public string                   videoId     { get; set; }
        public string                   channelId   { get; set; }
        public string                   description { get; set; }
        public int                      duration    { get; set; } 
        public DateTime                 publishDate { get; set; }
        public string                   thumbnail   { get; set; }

        public bool                     isWatched   { get; set; }
        public bool                     isFavorite  { get; set; }
        public bool                     isHidden    { get; set; }

        //public List<YTThumbnail>        thumbnails  { get; set; }
        public string[]                 tags        { get; set; }
    }
}
