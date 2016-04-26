using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Models;

namespace YUP.App.Services
{
    /// <summary>
    /// interface used for managing settings in application
    /// </summary>
    public interface IYupSettings
    {
        string            appPath { get; set; }
        YupMode           appMode { get; set; }

        bool              checkAppFolderPath();
        bool              createNewSettingsFile(string path=null, SavedSettings newSettings=null);
        SavedSettings     loadAppSettings();
        bool              saveAppSettings(string path, SavedSettings settings2save);
        bool              createAppFolderStructure();
    }
}
