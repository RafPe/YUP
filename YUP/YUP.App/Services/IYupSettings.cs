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
        string  appPath { get; set; }    
        YupMode appMode { get; set; }

        YTChannel   defaultChannel      { get; set;}
        string      defaultRepoLocation { get; set;}

        bool checkAppFolderPath();
        bool createAppFolderStructure();

    }
}
