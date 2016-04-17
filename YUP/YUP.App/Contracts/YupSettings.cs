using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Services;

namespace YUP.App.Contracts
{
    public class YupSettings : IYupSettings
    {
        public string   appPath { get; set; }
        public YupMode  appMode { get; set; }

        public bool checkAppFolderPath()
        {
            return true;
        }

        public bool createAppFolderStructure()
        {
            return true;
        }
    }
}
