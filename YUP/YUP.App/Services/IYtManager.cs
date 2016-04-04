using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;

namespace YUP.App.Services
{
    public interface IYtManager
    {
        Task<string>                GetChannelIdAsync(string ytUsername);
        List<object>                GetChannelStatistcs(string ytUsername);
        List<SearchResult>          GetVideosFromChannel(string ytChannelId);
        Task<List<SearchResult>>    GetVideosFromChannelAsync(string ytChannelId);
        string                      GetChannelIdForUserName(string username);
    }
}
