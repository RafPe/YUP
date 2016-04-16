using System;
using YUP.App.MediaPlayers;

namespace YUP.App.Events
{
    public class PlayerStateChangedArgs : EventArgs
    {
        public MediaPlaybackState   playbackState   { get; set; }
        public string               videoId         { get; set; }
    }
}
