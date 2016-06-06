using System.Collections.Generic;

namespace YUP.App.Models
{
    /// <summary>
    /// Class which we use for keeping our repository on disk/storage 
    /// </summary>
    public class AppRepository
    {
        public  string           repositoryVersion    { get; set; } // Used to determine if upgrade is needed

        public List<string>      categories           { get; set; }
        public List<YTVideo>     ytVideos             { get; set; }
        public List<YupItem>     yupItems             { get; set; }
        public List<YTChannel>   ytChannels           { get; set; }


        public AppRepository()
        {
            // Initialize objects 
            ytVideos    = new List<YTVideo>();
            yupItems    = new List<YupItem>();
            ytChannels  = new List<YTChannel>();
            categories  = new List<string>() {"default"};

            repositoryVersion = "";
        }
    }
}
