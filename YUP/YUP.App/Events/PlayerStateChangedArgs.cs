using System;
using YUP.App.MediaPlayers;

namespace YUP.App.Events
{
    public class PlayerStateChangedArgs : EventArgs
    {
        public MediaPlaybackState   PlaybackState   { get; set; }
        public string               VideoId         { get; set; }
    }
}
