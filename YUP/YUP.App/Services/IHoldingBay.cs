using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHoldingBay
    {
        void    AddEntry(string key, object obj);
        object  GetEntry(string key, bool remove=true);
    }
}
