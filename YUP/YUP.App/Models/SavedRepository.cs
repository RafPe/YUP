using System.Collections.Generic;

namespace YUP.App.Models
{
    /// <summary>
    /// Class which we use for keeping our repository on disk/storage 
    /// </summary>
    public class SavedRepository
    {
        public  IEnumerable<YTVideo>     ytVideos             { get; set; }
        public  IEnumerable<YupItem>     yupItems             { get; set; }
        public  IEnumerable<YTChannel>   ytChannels           { get; set; }
    }
}
