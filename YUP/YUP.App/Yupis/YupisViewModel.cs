﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
        }
    }
}
