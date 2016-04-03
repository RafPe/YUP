using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Player
{
    public class PlayerViewModel: BindableBase
    {
        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
        }
    }
}
