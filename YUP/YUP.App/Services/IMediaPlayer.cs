using System;
using YUP.App.Events;

namespace YUP.App.Services
{
    public interface IMediaPlayer
    {
        /// <summary>
        /// This property defines our video provider i.e. youtube , vimeo 
        /// which will allow for dynamic player load based on video.
        /// </summary>
        string video_provider { get; }

        /// <summary>
        /// This event handler is used to notify main application 
        /// that something has happened.
        /// </summary>
        event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        /// <summary>
        /// Property with current video source being played
        /// </summary>
        string currentVideoId { get; }

        /// <summary>
        /// Get current player width
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Get current player height
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Method sets player size
        /// </summary>
        /// <param name="w"> desired width</param>
        /// <param name="h"> desired height</param>
        void mediaSetPlayerSize(int w, int h);

        /// <summary>
        /// Get current player time (in seconds) from the video
        /// </summary>
        /// <returns> seconds from the beginning of video</returns>
        int mediaGetCurrentTime();

        /// <summary>
        /// Loads video by given ID
        /// </summary>
        /// <param name="videoId"> video Id </param>
        /// <param name="startFrom"> offset to start from in seconds</param>
        /// <param name="videoquality"> quality of loaded video </param>
        void mediaLoadVideo(string videoId, int startFrom, string videoquality = "default");

        /// <summary>
        /// Loads video by given ID
        /// </summary>
        /// <param name="videoId"> video Id </param>
        /// <param name="videoquality"> quality of loaded video </param>
        void mediaLoadVideo(string videoId, string videoquality = "default");

        /// <summary>
        /// Toggles between play and pause
        /// </summary>
        void mediaPlayPause();

        /// <summary>
        /// Pause currently playing video
        /// </summary>
        void mediaPause();

        /// <summary>
        /// Play currently playing video
        /// </summary>
        void mediaPlay();

        //TODO dont forget about proper handling of events
        //public void OnClosed(object sender, RoutedEventArgs e)
        //{
        //    if (Closed != null)
        //    {
        //        Closed(this, e);
        //    }
        //}
    }

}
