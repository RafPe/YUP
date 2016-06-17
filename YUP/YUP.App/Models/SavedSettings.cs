using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YUP.App.Models
{
    /// <summary>
    /// Class used for storing our application settings 
    /// </summary>
    public class SavedSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public YupMode appMode { get; set; }
        public string  appPath { get; set; }
    }
}
