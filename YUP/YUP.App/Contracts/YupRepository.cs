using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using Newtonsoft.Json;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupRepository : IYupRepository
    {
        private IYupSettings             _yupSettings;

        public  IEnumerable<YTVideo>     ytVideos             { get; set; }
        public  IEnumerable<YupItem>     yupItems             { get; set; }
        public  IEnumerable<YTChannel>   ytChannels           { get; set; }

        public  YTChannel                currentlySelected    { get; set; }
                                                              
        private string                   jsonChannelsPath     { get; set; }


        public YupRepository(IYupSettings yupSettings)
        {
            _yupSettings        = yupSettings;

            //TODO: #3
            //_eventBus.SubscribeEvent("VideoIdChanged", VideoIdChangedHandler);
            //_eventBus.SubscribeEvent("VideoIdChanged", VideoIdChangedHandler);


            jsonChannelsPath    = String.Format(@"{0}\channels.yup", _yupSettings.appPath);
        }

        public void LoadRepository()
        {
           
        }

        /// <summary>
        /// Saves current repository to location
        /// </summary>
        public void SaveRepository()
        {

            // Save channels 
            if (!ReferenceEquals(null, ytChannels))
            {
                var jsonChannels     = JsonConvert.SerializeObject(ytChannels);
                
                File.WriteAllText(jsonChannelsPath, jsonChannels);
            }
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
