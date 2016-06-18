using System;
using System.Collections.Generic;
using YUP.App.Base;

namespace YUP.App.Models
{
    public class YTChannel : YtBase
    {

        public string   user             { get; set; }
        public string   friendlyName     { get; set; }
        public string   videosCount      { get; set; }
        public DateTime dtAdded          { get; set; }

        public YTChannel()
        {
            user            = "";
            friendlyName    = "";
            description     = "";
            category        = "";
            tags            = new List<string>();        }
    }
}
