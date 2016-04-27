using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using YUP.App.Base;
using YUP.App.Events;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupRepository : IYupRepository
    {
        private IYupSettings             _yupSettings;
        private IEventBus                _eventBus;

        public List<YTVideo>     ytVideos             { get; set; }
        public List<YupItem>     yupItems             { get; set; }
        public List<YTChannel>   ytChannels           { get; set; }

        public  YTChannel                currentlySelected    { get; set; }

        EventBusHandler                  channelAdded;
        EventBusHandler                  channelRemoved;


        public YupRepository(IYupSettings yupSettings, IEventBus eventBus)
        {
            _yupSettings        = yupSettings;
            _eventBus           = eventBus;

            // Register event publications 
            _eventBus.PublishEvent(EventOnBus.channelAdded, channelAdded);
            _eventBus.PublishEvent(EventOnBus.channelRemoved, channelRemoved);

            // Initialize objects 
            ytVideos   = new List<YTVideo>();
            yupItems   = new List<YupItem>();
            ytChannels = new List<YTChannel>();

            //TODO: #3
            //_eventBus.SubscribeEvent("VideoIdChanged", VideoIdChangedHandler);
            //_eventBus.SubscribeEvent("VideoIdChanged", VideoIdChangedHandler);
        }

        /// <summary>
        /// Loads repository from storage
        /// </summary>
        public void LoadRepository()
        {
            // If our settings file does not exist let's create it
            if (!File.Exists($@"{_yupSettings.appPath}\{AppBase.fileRepository}")) SaveRepository();

            var loadedRepository = JsonConvert.DeserializeObject<SavedRepository>(File.ReadAllText($@"{_yupSettings.appPath}\{AppBase.fileRepository}"));

            // Assign values from loaded repository
            this.ytChannels = loadedRepository.ytChannels;
            this.ytVideos   = loadedRepository.ytVideos;
            this.yupItems   = loadedRepository.yupItems;


        }

        /// <summary>
        /// Saves current repository to location
        /// </summary>
        public void SaveRepository()
        {
            // Get our current repository prepared for saving
            SavedRepository savedRepository = new SavedRepository()
            {
                ytChannels = this.ytChannels,
                ytVideos   = this.ytVideos,
                yupItems   = this.yupItems 
            };

            var jsonRepo = JsonConvert.SerializeObject(savedRepository);

            File.WriteAllText($@"{_yupSettings.appPath}\{AppBase.fileRepository}", jsonRepo);
        }

        /// <summary>
        /// Adds new youtube channel to our repository
        /// </summary>
        /// <param name="channel">Defined youtube channel</param>
        public void AddChannel(YTChannel channel)
        {
            if (ReferenceEquals(channel, null)) return;

            ytChannels.Add(channel);
        }

        public void Editchannel(YTChannel channel)
        {
            //TODO ? Do we need to edit channels ? 
        }

        /// <summary>
        /// Removes youtube channel from our repository
        /// </summary>
        /// <param name="channel">Channel to be removed </param>
        public void RemoveChannel(YTChannel channel)
        {
            if (ReferenceEquals(channel, null)) return;

            ytChannels.Remove(channel);
        }

        /// <summary>
        /// Adds yupi to our repository
        /// </summary>
        /// <param name="yupi">Yupi to be added</param>
        public void AddYupi(YupItem yupi)
        {
            if (ReferenceEquals(yupi, null)) return;

            yupItems.Add(yupi);
        }

        public void EditYupi(YupItem yupi)
        {
            //TODO # We need to do edit of yupis
        }

        /// <summary>
        /// Removes yupi from our repository
        /// </summary>
        /// <param name="yupi">Yupi to be removed</param>
        public void RemoveYupi(YupItem yupi)
        {
            if (ReferenceEquals(yupi, null)) return;

            yupItems.Remove(yupi);
        }
    }
}
