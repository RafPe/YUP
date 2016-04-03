using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Yupis
{
    public class YupisViewModel : BindableBase
    {
        public string testmessage { get; set; }


        public YupisViewModel()
        {
            testmessage = String.Format("{0:O}", DateTime.Now);
        }
    }
}
