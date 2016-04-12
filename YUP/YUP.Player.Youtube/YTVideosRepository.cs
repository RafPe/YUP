using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using           a;
using Newtonsoft.Json.Linq;
using RestSharp;
using YUP.Core.Data;
using YUP.Core.Services;

namespace YUP.Player.Youtube
{
    /// <summary>
    /// Class used by YUP to interoperate with YouTube
    /// </summary>
    public class YTVideosRepository :IVideosRepository
    {
        private readonly YouTubeService _youtubeService;


        public YTVideosRepository()
        {
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //TODO 1: Change API key to be read from app config file
                ApiKey              = "AIzaSyBR0HTLK71LpprSpWvxa70thZ6Bx4KDd44",
                ApplicationName     = "Videopedia"//this.GetType().ToString()
            });
        }

        public string GetchannelId(string username)
        {
            RestClient client = new RestClient($"https://www.googleapis.com/youtube/v3/channels?key={_youtubeService.ApiKey}&forUsername={username}&part=id");
            RestRequest restRequest = new RestRequest(Method.GET);

            RestResponse response = (RestResponse) client.Execute(restRequest);
            var content = response.Content; // raw content as string

            JObject o = JObject.Parse(content);

            string name = (string)o.SelectToken("items[0].id");

            return name??null;
        }

        public List<object> GetChannelStatistcs(string username)
        {
            var channelStatisticsReq = _youtubeService.Channels.List("statistics");
            channelStatisticsReq.ForUsername = username;

            var channelStatisticsResp = channelStatisticsReq.Execute();

            List<object> tmpChannels = new List<object>();

            foreach (Channel channel in channelStatisticsResp.Items)
            {
                tmpChannels.Add(channel);
            }

            return tmpChannels;
        }

        private List<YTVideo> ProcessSearchResults(IList<SearchResult> searchResults)
        {
            List<YTVideo> templist = new List<YTVideo>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchResults)
            {             
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":

                        YTVideo tmpobj = new YTVideo()
                        {
                            VideoId = searchResult.Id.VideoId,
                            ChannelId = searchResult.Id.ChannelId,
                            Author = searchResult.Snippet.ChannelTitle,
                            Provider = "youtube",
                            PublishedAt = searchResult.Snippet.PublishedAt ?? new DateTime(1900, 1, 1),
                            Title = searchResult.Snippet.Title,
                            Thumbnails = new List<YTThumbnail>() { new YTThumbnail()
                                {
                                    size = "Default",
                                    url = searchResult.Snippet.Thumbnails.Default__?.Url??"empty"
                                },
                                new YTThumbnail()
                                {
                                    size = "Medium",
                                    url = searchResult.Snippet.Thumbnails.Medium?.Url??"empty"
                                },
                                new YTThumbnail()
                                {
                                    size = "High",
                                    url = searchResult.Snippet.Thumbnails.High?.Url??"empty"
                                },
                                new YTThumbnail()
                                {
                                    size = "Maxres",
                                    url = searchResult.Snippet.Thumbnails.Maxres?.Url??"empty"
                                },
                                new YTThumbnail()
                                {
                                    size = "Standard",
                                    url = searchResult.Snippet.Thumbnails.Standard?.Url??"empty"
                                }

                        }
                        };

                        // Add video object to list
                        templist.Add(tmpobj);
                        break;

                }
            }

            return templist;

        }

        public List<YTVideo> GetVideosFromChannel(string channelId)
        {

            List<YTVideo> res = new List<YTVideo>();

            _youtubeService.Search.List("snippet");
            string nextpagetoken = " ";

            while (nextpagetoken != null)
            {
                var searchListRequest = _youtubeService.Search.List("snippet");
                searchListRequest.MaxResults = 50;
                searchListRequest.ChannelId = channelId;
                searchListRequest.PageToken = nextpagetoken;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = searchListRequest.Execute();

                // Process  the video responses 
                res.AddRange(ProcessSearchResults(searchListResponse.Items));


                nextpagetoken = searchListResponse.NextPageToken;

            }



            // Process  and return the resuls 

            return res;

        }

        public async Task<List<YTVideo>> GetVideosFromChannelAsync(string channelId)
        {
            List<YTVideo> res = new List<YTVideo>();

            _youtubeService.Search.List("snippet");
            string nextpagetoken = " ";

            while (nextpagetoken != null)
            {
                var searchListRequest = _youtubeService.Search.List("snippet");
                searchListRequest.MaxResults = 50;
                searchListRequest.ChannelId = channelId;
                searchListRequest.PageToken = nextpagetoken;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();

                // Process  the video responses 
                res.AddRange(ProcessSearchResults(searchListResponse.Items));


                nextpagetoken = searchListResponse.NextPageToken;

            }



            // Process  and return the resuls 

            return res;
        }
    }
}
