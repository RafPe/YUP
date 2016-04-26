using System;
using System.IO;
using Newtonsoft.Json;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupSettings : IYupSettings
    {

        private string _appPath { get; set; }
        public  string appPath
        {
            get
            {
                // Return default if we dont have it yet set up 
                if (string.IsNullOrEmpty(_appPath))
                {
                    _appPath =  $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Yup";
                }

                return _appPath;
            }
            set
            {
                _appPath = value;
            }
        }

        public YupMode  appMode                 { get; set; }


        public YupSettings()
        {

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

        /// <summary>
        /// Creates new setting file in given path
        /// </summary>
        /// <param name="path"> Path to save the file in </param>
        /// <param name="newSettings">Optional file with new settings</param>
        /// <returns></returns>
        public bool createNewSettingsFile(string path=null, SavedSettings newSettings = null)
        {

            if (path == null) path = appPath;

            if (newSettings == null) newSettings = new SavedSettings()
            {
                appPath = appPath
            };

            var jsonSettings = JsonConvert.SerializeObject(newSettings);

            File.WriteAllText($@"{appPath}\settings.yup", jsonSettings);

            return true;

        }

        public SavedSettings loadAppSettings()
        {
            // If our settings file does not exist let's create it
            if (!File.Exists($@"{appPath}\settings.yup"))
            {
                this.checkAppFolderPath();

                this.createNewSettingsFile(this.appPath, new SavedSettings()
                {
                    appPath = appPath,
                    appMode = YupMode.Online
                });
            }

            var results = JsonConvert.DeserializeObject<SavedSettings>(File.ReadAllText($@"{appPath}\settings.yup"));

            return results;
        }

        public bool saveAppSettings(string path, SavedSettings settings2save)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates required folder structure
        /// </summary>
        /// <returns></returns>
        public bool createAppFolderStructure()
        {
            //TODO: # Create folder structure
            return true;
        }
    }
}
