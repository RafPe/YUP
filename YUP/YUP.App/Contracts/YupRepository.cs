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

        public  IEnumerable<YTVideo>     ytVideos             { get; set; }
        public  IEnumerable<YupItem>     yupItems             { get; set; }
        public  IEnumerable<YTChannel>   ytChannels           { get; set; }

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

        public void AddChannel()
        {
            throw new NotImplementedException();
        }

        public void Editchannel()
        {
            throw new NotImplementedException();
        }

        public void RemoveChannel()
        {
            throw new NotImplementedException();
        }

        public void LoadYupis()
        {
            throw new NotImplementedException();
        }

        public void AddYupi()
        {
            throw new NotImplementedException();
        }

        public void EditYupi()
        {
            throw new NotImplementedException();
        }

        public void RemoveYupi()
        {
            throw new NotImplementedException();
        }
    }
}
