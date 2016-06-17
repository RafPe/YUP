using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class HoldingBay : IHoldingBay
    {
        private Dictionary<string, object> _holderDictionary;

        public HoldingBay()
        {
            _holderDictionary = new Dictionary<string, object>();
        }  

        /// <summary>
        /// Adds object to our dictionary by key and replaces 
        /// existing if key already exists 
        /// </summary>
        /// <param name="key">key identifier</param>
        /// <param name="obj">object to be added</param>
        public void AddEntry(string key, object obj)
        {
            if (_holderDictionary.ContainsKey(key))
            {
                _holderDictionary[key] = obj;
            }
            else
            {
                _holderDictionary.Add(key, obj);
            }
        }

        /// <summary>
        /// Gets object from our dictionary 
        /// and optionally removes it if required in param
        /// </summary>
        /// <param name="key">key identifier</param>
        /// <param name="remove">Optional parameter to remove from our dictionary</param>
        /// <returns></returns>
        public object GetEntry(string key,bool remove=true)
        {
            object tmpReturn = null;

            if (_holderDictionary.ContainsKey(key))
            {
                tmpReturn = _holderDictionary[key];

                if (remove) _holderDictionary.Remove(key);
            }

            return tmpReturn;
        }
    }
}
