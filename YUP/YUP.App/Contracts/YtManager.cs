using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using YUP.App.Models;
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

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = searchListRequest.Execute();

                    // Process  the video responses 
                    res.AddRange(searchListResponse.Items);

                    nextpagetoken = searchListResponse.NextPageToken;

                }

                return res;

            });
        }

        public Task<string> GetChannelIdAsync(string ytUsername) // No async because the method does not need await
        {
            return Task.Run(() =>
            {
                RestClient client = new RestClient($"https://www.googleapis.com/youtube/v3/channels?key={_youtubeService.ApiKey}&forUsername={ytUsername}&part=id");
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
            });
        }

        public string GetChannelIdForUserName(string username)
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

        public List<object> GetChannelStatistcs(string ytUsername)
        {
            var channelStatisticsReq            = _youtubeService.Channels.List("statistics");
            channelStatisticsReq.ForUsername    = ytUsername;

            var channelStatisticsResp = channelStatisticsReq.Execute();

            List<object> tmpChannels = new List<object>();

            foreach (Channel channel in channelStatisticsResp.Items)
            {
                tmpChannels.Add(channel);
            }

            return tmpChannels;
        }

        //private List<YTVideo> ProcessSearchResults(IList<SearchResult> searchResults)
        //{
        //    List<YTVideo> templist = new List<YTVideo>();

        //    // Add each result to the appropriate list, and then display the lists of
        //    // matching videos, channels, and playlists.
        //    foreach (var searchResult in searchResults)
        //    {
        //        switch (searchResult.Id.Kind)
        //        {
        //            case "youtube#video":

        //                YTVideo tmpobj = new YTVideo()
        //                {
        //                    VideoId = searchResult.Id.VideoId,
        //                    ChannelId = searchResult.Id.ChannelId,
        //                    Author = searchResult.Snippet.ChannelTitle,
        //                    Provider = "youtube",
        //                    PublishedAt = searchResult.Snippet.PublishedAt ?? new DateTime(1900, 1, 1),
        //                    Title = searchResult.Snippet.Title,
        //                    Thumbnails = new List<YTThumbnail>() { new YTThumbnail()
        //                        {
        //                            size = "Default",
        //                            url = searchResult.Snippet.Thumbnails.Default__?.Url??"empty"
        //                        },
        //                        new YTThumbnail()
        //                        {
        //                            size = "Medium",
        //                            url = searchResult.Snippet.Thumbnails.Medium?.Url??"empty"
        //                        },
        //                        new YTThumbnail()
        //                        {
        //                            size = "High",
        //                            url = searchResult.Snippet.Thumbnails.High?.Url??"empty"
        //                        },
        //                        new YTThumbnail()
        //                        {
        //                            size = "Maxres",
        //                            url = searchResult.Snippet.Thumbnails.Maxres?.Url??"empty"
        //                        },
        //                        new YTThumbnail()
        //                        {
        //                            size = "Standard",
        //                            url = searchResult.Snippet.Thumbnails.Standard?.Url??"empty"
        //                        }

        //                }
        //                };

        //                // Add video object to list
        //                templist.Add(tmpobj);
        //                break;

        //        }
        //    }

        //    return templist;

        //}

        public List<SearchResult> GetVideosFromChannel(string ytChannelId )
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
    }
}
