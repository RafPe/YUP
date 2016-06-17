using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using YUP.App.Base;
using YUP.App.Events;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupRepository : IYupRepository
    {
        #region IoC 
        private IYupSettings             _yupSettings;
        private IEventBus                _eventBus;
        #endregion

        #region Public properties

        public AppRepository     appRepo { get; set; }

        #endregion

        #region cTor
        public YupRepository(IYupSettings yupSettings, IEventBus eventBus)
        {
            _yupSettings        = yupSettings;
            _eventBus           = eventBus;

            this.LoadRepository();

        }
        #endregion  

        #region Repository actions

        /// <summary>
        /// Loads repository from storage
        /// </summary>
        public void LoadRepository()
        {
            // If our settings file does not exist let's create it
            if (!File.Exists($@"{_yupSettings.appPath}\{AppBase.fileRepository}")) SaveRepository();

            var loadedRepository = JsonConvert.DeserializeObject<AppRepository>(File.ReadAllText($@"{_yupSettings.appPath}\{AppBase.fileRepository}"));

            if (!loadedRepository.categories.Contains("default")) loadedRepository.categories.Add("default");

            // Assign values from loaded repository
            appRepo = loadedRepository;
        }

        /// <summary>
        /// Saves current repository to location
        /// </summary>
        public void SaveRepository()
        {
            if(ReferenceEquals(appRepo,null)) appRepo = new AppRepository();

            File.WriteAllText($@"{_yupSettings.appPath}\{AppBase.fileRepository}", JsonConvert.SerializeObject(appRepo) );
        }

        #endregion

        #region Channels

        /// <summary>
        /// Adds new youtube channel to our repository
        /// </summary>
        /// <param name="channel">Defined youtube channel</param>
        public void AddChannel(YTChannel channel)
        {
            if (ReferenceEquals(channel, null)) return;

            appRepo.ytChannels.Add(channel);

            _eventBus.RaiseEvent(EventOnBus.channelAdded, this, new EventBusArgs() { Item = channel });
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

            appRepo.ytChannels.Remove(channel);

            _eventBus.RaiseEvent(EventOnBus.channelRemoved, this, new EventBusArgs() { Item = channel });
        }

        public List<YTChannel> GetAllYtChannels()
        {
            return appRepo.ytChannels;
        }

        #endregion

        #region Yupis

        /// <summary>
        /// Adds yupi to our repository
        /// </summary>
        /// <param name="yupi">Yupi to be added</param>
        public void AddYupi(YupItem yupi)
        {
            if (ReferenceEquals(yupi, null)) return;

            appRepo.yupItems.Add(yupi);
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

            appRepo.yupItems.Remove(yupi);
        }
        #endregion

        #region Category

        public void AddCategory(string cat)
        {
            appRepo.categories.Add(cat);
        }

        public List<string> GetAllCategories()
        {
            return appRepo.categories;
        }

        public void EditCategory(string cat, string newCat)
        {
            appRepo.categories[appRepo.categories.FindIndex(ind => ind.Equals(cat))] = newCat;
        }

        public void RemoveCategory(string cat)
        {
            appRepo.categories.Remove(cat);
        }

        #endregion
    }
}
