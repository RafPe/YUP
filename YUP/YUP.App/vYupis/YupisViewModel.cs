using System;
using System.ComponentModel;
using YUP.App.Contracts;
using YUP.App.Services;

namespace YUP.App.vYupis
{
    public class YupisViewModel : BindableBase
    {

        private IYupRepository    _yupiManager;
        private IEventBus       _eventBus;

        public event EventBusHandler VideoIdChangedHandler;

        public  string          testmessage { get; set; }
        //TODO:Player tests - remove ...
        public  RelayCommand    test        { get; private set; }



        public YupisViewModel(IYupRepository yupiManager,IEventBus eventBus)
        {
            _eventBus       = eventBus;
            _yupiManager    = yupiManager;

            test=new RelayCommand(onTest);
            _eventBus.PublishEvent("VideoIdChanged", VideoIdChangedHandler);


            testmessage = String.Format("{0:O}", DateTime.Now);
        }

        private void onTest()
        {

            _eventBus.RaiseEvent("VideoIdChanged", this,new EventBusArgs() {Item = "b9FC9fAlfQE" });

        }

        public async void LoadData()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
        }
    }
}
