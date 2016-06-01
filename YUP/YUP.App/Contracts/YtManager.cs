using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YtManager : IYtManager
    {
        private readonly YouTubeService _youtubeService;


        public YtManager()
        {
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //TODO 1: Change API key to be read from app config file
                ApiKey = "AIzaSyBR0HTLK71LpprSpWvxa70thZ6Bx4KDd44",
                ApplicationName = "Videopedia"//this.GetType().ToString()
            });
        }

        public List<SearchResult>       GetVideosFromChannel(string ytChannelId)
        {

            List<SearchResult> res = new List<SearchResult>();

            _youtubeService.Search.List("snippet");


            string nextpagetoken = " ";

            while (nextpagetoken != null)
            {
                var searchListRequest = _youtubeService.Search.List("snippet");
                searchListRequest.MaxResults = 50;
                searchListRequest.ChannelId = ytChannelId;
                searchListRequest.PageToken = nextpagetoken;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = searchListRequest.Execute();

                // Process  the video responses 
                res.AddRange(searchListResponse.Items);

                nextpagetoken = searchListResponse.NextPageToken;

            }

            return res;

        }

        public Task<List<SearchResult>> GetVideosFromChannelAsync(string ytChannelId)
        {

            return Task.Run(() =>
            {
                List<SearchResult> res = new List<SearchResult>();

                string nextpagetoken = " ";

                while (nextpagetoken != null)
                {
                    var searchListRequest = _youtubeService.Search.List("snippet");
                    searchListRequest.MaxResults = 50;
                    searchListRequest.ChannelId = ytChannelId;
                    searchListRequest.PageToken = nextpagetoken;
                    searchListRequest.Type      = "video";

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = searchListRequest.Execute();

                    // Process  the video responses 
                    res.AddRange(searchListResponse.Items);

                    nextpagetoken = searchListResponse.NextPageToken;

                }

                return res;

            });
        }

        /// <summary>
        /// Function responsible for parsing URL containing videoId
        /// Source: http://stackoverflow.com/a/27728417/2476347
        /// </summary>
        /// <param name="url">URL containing video ID from youtube</param>
        /// <returns></returns>
        public string GetVideoIdFromUrl(string url)
        {
            var r = new Regex(@"^.*(?:(?:youtu\.be\/|v\/|vi\/|u\/\w\/|embed\/)|(?:(?:watch)?\?v(?:i)?=|\&v(?:i)?=))(?<videoId>[^#\&\?]*).*");

            var match = r.Match(url);

            if (match.Success && !String.IsNullOrEmpty(match.Groups["videoId"].Value.ToLower()))
            {
                return match.Groups["videoId"].Value.ToLower();
            }

            return null;
        }

        /// <summary>
        /// Function responsible for querying channel Id from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string       GetChannelIdFromUrl(string url)
        {
            var final = url.Substring(url.LastIndexOf('/') + 1);

            return final;
        }

        //public Task<string> GetChannelIdAsync(string ytUsername) // No async because the method does not need await
        //{
        //    return Task.Run(() =>
        //    {
        //        RestClient client = new RestClient($"https://www.googleapis.com/youtube/v3/channels?key={_youtubeService.ApiKey}&forUsername={ytUsername}&part=id");
        //        RestRequest restRequest = new RestRequest(Method.GET);

        //        try
        //        {
        //            RestResponse response = (RestResponse)client.Execute(restRequest);
        //            var content = response.Content; // raw content as string

        //            JObject o = JObject.Parse(content);

        //            string name = (string)o.SelectToken("items[0].id");

        //            return name;
        //        }
        //        catch (Exception ex)
        //        {

        //            return null;

        //        }
        //    });
        //}



        public Channel GetChannelStatistcs(string ytUsername)
        {
            var channelStatisticsReq         = _youtubeService.Channels.List("statistics,snippet");
            channelStatisticsReq.ForUsername = ytUsername;
            channelStatisticsReq.MaxResults = 1;

            var channelStatisticsResp = channelStatisticsReq.Execute();

            if (channelStatisticsResp.Items.Count > 0 ) return channelStatisticsResp.Items[0] ;

            return null;
        }

        public Task<Channel> GetChannelStatistcsAsync(string ytUsername)
        {
            throw new NotImplementedException();
        }


        public Task<Channel> GetChannelStatistcsByChannelIdAsync(string ytChannelId)
        {
            throw new NotImplementedException();
        }

        public List<Channel>       GetChannelSnippet(string ytUsername)
        {
            throw new NotImplementedException();
        }

        public Task<List<Channel>> GetChannelSnippetAsync(string ytUsername)
        {
            throw new NotImplementedException();
        }

        public string       GetChannelIdForUser(string username)
        {
            RestClient client = new RestClient($"https://www.googleapis.com/youtube/v3/channels?key={_youtubeService.ApiKey}&forUsername={username}&part=id");
            RestRequest restRequest = new RestRequest(Method.GET);

            try
            {
                RestResponse response = (RestResponse)client.Execute(restRequest);
                var content = response.Content; // raw content as string

                JObject o = JObject.Parse(content);

                string name = (string)o.SelectToken("items[0].id");

                return name;
            }
            catch (Exception ex)
            {

                return null;

            }
            
        }

        public Task<string> GetChannelIdForUserAsync(string username)
        {
            return Task.Run(() =>
            {
                RestClient client =
                    new RestClient(
                        $"https://www.googleapis.com/youtube/v3/channels?key={_youtubeService.ApiKey}&forUsername={username}&part=id");
                RestRequest restRequest = new RestRequest(Method.GET);

                try
                {
                    RestResponse response = (RestResponse) client.Execute(restRequest);
                    var content = response.Content; // raw content as string

                    JObject o = JObject.Parse(content);

                    string name = (string) o.SelectToken("items[0].id");

                    return name;
                }
                catch (Exception ex)
                {

                    return null;

                }
            });
        }

        public Channel GetChannelStatistcsByUser(string ytUsername)
        {
            var channelStatisticsReq = _youtubeService.Channels.List("id,snippet,statistics,contentDetails,topicDetails");
            channelStatisticsReq.ForUsername = ytUsername;
            channelStatisticsReq.MaxResults = 1;

            var channelStatisticsResp = channelStatisticsReq.Execute();

            if (channelStatisticsResp.Items.Count > 0) return channelStatisticsResp.Items[0];

            return null;
        }

        public Task<Channel> GetChannelStatistcsByUserAsync(string ytUsername)
        {
            throw new NotImplementedException();
        }

        public Channel GetChannelStatistcsByChannelId(string ytChannelId)
        {
            var channelStatisticsReq = _youtubeService.Channels.List("id,snippet,statistics,contentDetails,topicDetails");
            channelStatisticsReq.Id = ytChannelId;
            channelStatisticsReq.MaxResults = 1;

            var channelStatisticsResp = channelStatisticsReq.Execute();

            if (channelStatisticsResp.Items.Count > 0) return channelStatisticsResp.Items[0];

            return null;
        }
    }
}
