using System.Collections.Generic;

namespace YUP.App.Models
{
    /// <summary>
    /// Class which we use for keeping our repository on disk/storage 
    /// </summary>
    public class SavedRepository
    {
        public List<YTVideo>     ytVideos             { get; set; }
        public List<YupItem>     yupItems             { get; set; }
        public List<YTChannel>   ytChannels           { get; set; }
    }
}
