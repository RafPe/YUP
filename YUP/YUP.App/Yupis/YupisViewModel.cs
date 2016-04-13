using System;
using System.ComponentModel;
using YUP.App.Services;

namespace YUP.App.Yupis
{
    public class YupisViewModel : BindableBase
    {

        private IYupiManager    _yupiManager;
        public  string          testmessage { get; set; }
        //TODO:Player tests - remove ...
        public  RelayCommand    test        { get; private set; }



        public YupisViewModel(IYupiManager yupiManager)
        {
            _yupiManager = yupiManager;
            test=new RelayCommand(onTest);

            testmessage = String.Format("{0:O}", DateTime.Now);
        }

        private void onTest()
        {
            var yyy = ContainerHelper.GetService<IMediaPlayer>("youtube");
            yyy.mediaLoadVideo("b9FC9fAlfQE");
        }

        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
        }
    }
}
