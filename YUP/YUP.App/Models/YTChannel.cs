namespace YUP.App.Models
{
    public class YTChannel
    {
        public string   channelId               { get; set; }
        public string   channelUser             { get; set; }
        public string   channelFriendlyName     { get; set; }

        public bool     isFavorite              { get; set; }
        public bool     isHidden                { get; set; }
        public bool     isSelected              { get; set; } //Used for multiple items editing 
    }
}
