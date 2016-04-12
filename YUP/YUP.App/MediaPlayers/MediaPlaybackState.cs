namespace YUP.App.MediaPlayers
{
    /// <summary>
    /// This enum defines states that our video playback can have
    /// </summary>
    public enum MediaPlaybackState
    {
        notstarted,
        ended,
        playing,
        paused,
        buffering,
        videocued

    }
}
