using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Models
{
    /// <summary>
    /// Class used for storing our application settings 
    /// </summary>
    public class SavedSettings
    {
        public YupMode appMode { get; set; }
        public string  appPath { get; set; }
    }
}
