using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Events
{
    public static class EventOnBus
    {
        public static readonly string videoIdChanged = "videoIdChanged";
        public static readonly string channelRemoved = "channelRemoved";
        public static readonly string channelAdded   = "channelAdded";
        public static readonly string channelEdited  = "channelEdited";
    }
}
