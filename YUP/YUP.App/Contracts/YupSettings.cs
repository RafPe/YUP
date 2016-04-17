using System;
using System.IO;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupSettings : IYupSettings
    {
        public string   appPath                 { get; set; }
        public YupMode  appMode                 { get; set; }

        private string  _defultApplocation      { get; set; }

        public YupSettings()
        {
            _defultApplocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Yup";

            if (String.IsNullOrEmpty(appPath))
            {
                appPath = _defultApplocation;
            }
        }

        /// <summary>
        /// Checks if our application folder exists and create if necessary
        /// </summary>
        /// <returns></returns>
        public bool checkAppFolderPath()
        {
            
            if (!(Directory.Exists(appPath)))
            {
                Directory.CreateDirectory(appPath);
            }
            
            return true;
        }

        public bool createAppFolderStructure()
        {
            return true;
        }
    }
}
