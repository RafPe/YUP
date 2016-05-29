using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;

namespace YUP.App.Services
{
    public interface IYtManager
    {
        string                      GetChannelIdFromUrl(string url);
        string                      GetVideoIdFromUrl(string url);

        string                      GetChannelIdForUser(string username);
        Task<string>                GetChannelIdForUserAsync(string username);

        Channel                     GetChannelStatistcs(string ytUsername);
        Task<Channel>               GetChannelStatistcsAsync(string ytUsername);

        List<Channel>               GetChannelSnippet(string ytUsername);
        Task<List<Channel>>         GetChannelSnippetAsync(string ytUsername);

        List<SearchResult>          GetVideosFromChannel(string ytChannelId);
        Task<List<SearchResult>>    GetVideosFromChannelAsync(string ytChannelId);


    }
}
