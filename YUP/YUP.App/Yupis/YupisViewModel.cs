using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YUP.App.Services;

namespace YUP.App.Yupis
{
    public class YupisViewModel : BindableBase
    {

        private IYupiManager _yupiManager;

        public string testmessage { get; set; }


        public YupisViewModel(IYupiManager yupiManager)
        {
            _yupiManager = yupiManager;

            testmessage = String.Format("{0:O}", DateTime.Now);
        }
    }
}
