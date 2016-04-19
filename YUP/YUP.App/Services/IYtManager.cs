using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;

namespace YUP.App.Services
{
    public interface IYtManager
    {
        string                      GetChannelIdFromUrl(string url);
        string                      GetChannelIdForUserName(string username);
        Task<string>                GetChannelIdForUserNameAsync(string username);
        Task<string>                GetChannelIdAsync(string ytUsername);

        List<Channel>               GetChannelStatistcs(string ytUsername);
        Task<List<Channel>>         GetChannelStatistcsAsync(string ytUsername);

        List<Channel>               GetChannelSnippet(string ytUsername);
        Task<List<Channel>>         GetChannelSnippetAsync(string ytUsername);

        List<SearchResult>          GetVideosFromChannel(string ytChannelId);
        Task<List<SearchResult>>    GetVideosFromChannelAsync(string ytChannelId);
        string                      GetVideoIdFromUrl(string url);

    }
}
