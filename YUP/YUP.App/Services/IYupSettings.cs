using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Services
{
    /// <summary>
    /// interface used for managing settings in application
    /// </summary>
    public interface IYupSettings
    {
        string  appPath { get; set; }    
        YupMode appMode { get; set; }

        bool checkAppFolderPath();
        bool createAppFolderStructure();

    }
}
