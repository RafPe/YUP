using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using Google.Apis.YouTube.v3.Data;
using MaterialDesignThemes.Wpf;
using YUP.App.Contracts;
using YUP.App.Dialogs;
using YUP.App.Events;
using YUP.App.Helpers;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vChannels
{
    public class ChannelsViewModel : BindableBase, IEventRegistrator
    {
        private IYupRepository  _yupRepository;
        private YTChannel       _selectedYtChannel;
        private IYtManager      _ytManager;
        private IEventBus       _eventBus;


        public ObservableCollection<YTChannel>  YtChannels { get; set; }

        public YTChannel SelectedYtChannel
        {
            get { return _selectedYtChannel; }
            set { _selectedYtChannel = value; }
        }

        public event EventBusHandler channelAdded;
        public event EventBusHandler channelRemoved;

        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        //public string       SearchBoxTerm           { get; set; } = "";

        public RelayCommand CardShareCmd            { get; private set; }
        public RelayCommand CardDeleteCmd           { get; private set; }
        public RelayCommand CardEditmd              { get; private set; }
        public RelayCommand CardFavoriteCmd         { get; private set; }
        public RelayCommand SearchBoxCmd            { get; private set; }



        internal CollectionViewSource CvsStaff { get; set; }
        public ICollectionView AllStaff
        {
            get { return CvsStaff.View; }
        }

        private string _searchBoxTerm;

        public string SearchBoxTerm
        {
            get { return this._searchBoxTerm; }
            set
            {
                this._searchBoxTerm = value;
                if (SearchBoxTerm.Length ==0 || SearchBoxTerm.Length >3 ) OnFilterChanged();
            }
        }

        private void OnFilterChanged()
        {
             CvsStaff.View.Refresh();
        }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="yupRepository"></param>
        /// <param name="ytManager"></param>
        /// <param name="eventbus"></param>
        public ChannelsViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus)
        {
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;
            _eventBus       = eventbus;

            YtChannels      = new ObservableCollection<YTChannel>();


            CvsStaff        = new CollectionViewSource();
            CvsStaff.Source = this.YtChannels;
            //CvsStaff.Filter += ApplyFilter;
            //CvsStaff.Filter += ApplyFilterUserName;

            CardShareCmd    = new RelayCommand(onCmdRemoveChannel);
            CardDeleteCmd   = new RelayCommand(onCmdRemoveChannel);
            CardEditmd      = new RelayCommand(onCmdRemoveChannel);
            CardFavoriteCmd = new RelayCommand(onCmdRemoveChannel);

            SearchBoxCmd    = new RelayCommand(klinkal);


        }

        private void ApplyFilterUserName(object sender, FilterEventArgs e)
        {
            YTChannel svm = (YTChannel)e.Item;

            if (string.IsNullOrWhiteSpace(this.SearchBoxTerm) || this.SearchBoxTerm.Length == 0 || this.SearchBoxTerm.Length < 3)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = (svm.user.IndexOf(SearchBoxTerm, StringComparison.OrdinalIgnoreCase)) >= 0;
            }
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {

            YTChannel svm = (YTChannel)e.Item;

            if (string.IsNullOrWhiteSpace(this.SearchBoxTerm) || this.SearchBoxTerm.Length == 0 || this.SearchBoxTerm.Length <3)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = (svm.description.IndexOf(SearchBoxTerm, StringComparison.OrdinalIgnoreCase)) >= 0;
            }
        }

        public async void onCmdRemoveChannel()
        {

            if (_selectedYtChannel == null) return;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DialogEditChannel()
            {
                DataContext = this
            };

            //show the dialog
            bool result = (bool) await DialogHost.Show(view, "RootDialog");

            if(result) YtChannels.Remove(SelectedYtChannel);

        }

        private bool IsUrl(string input)
        {

            var r = new Regex(@"^http(s)?.*");

            var match = r.Match(input);

            if (match.Success) return true;

            return false;

        }

        public async void klinkal()
        {
            // If we dont have anything we skip actions :) 
            if (string.IsNullOrWhiteSpace(SearchBoxTerm)) return;

            string tmpChannelID;
            Channel tmpChannelStats = null;

            if (IsUrl(SearchBoxTerm))
            {
                tmpChannelID = _ytManager.GetChannelIdFromUrl(SearchBoxTerm);

                if (tmpChannelID != null)
                {
                    tmpChannelStats = _ytManager.GetChannelStatistcsByChannelId(tmpChannelID);
                }

            }
            else
            {
                tmpChannelID = await _ytManager.GetChannelIdForUserAsync(SearchBoxTerm);

                if (tmpChannelID != null)
                {
                    tmpChannelStats = _ytManager.GetChannelStatistcsByUser(SearchBoxTerm);
                }

            }

            // bail out if we have not found anything 
            if (tmpChannelStats == null) return;



            var chann = new YTChannel()
            {
                description =
                    $"{tmpChannelStats.Snippet.Description.Substring(0, tmpChannelStats.Snippet.Description.Length < 100 ? tmpChannelStats.Snippet.Description.Length : 100).Trim()} ...",
                thumbnail = tmpChannelStats.Snippet.Thumbnails.High.Url,
                channelId = tmpChannelStats.Id,
                user = tmpChannelStats.Snippet.Title
            };

            _yupRepository.ytChannels.Add(chann);
            _yupRepository.SaveRepository();
            YtChannels.Add(chann);


            _eventBus.RaiseEvent(EventOnBus.channelAdded, this, new EventBusArgs() {Item = chann});

            // Lastly we refresh our collection source 

            SearchBoxTerm = "";

            CvsStaff.View.Refresh();

        }

        /// <summary>
        /// Method responsible for publishing this View specific events
        /// </summary>
        public void PublishEvents()
        {
            _eventBus.PublishEvent(EventOnBus.channelAdded, channelAdded);
            _eventBus.PublishEvent(EventOnBus.channelRemoved, channelRemoved);
        }


        /// <summary>
        /// Method responsible for subscribing this View specific events
        /// </summary>
        public void SubscribeEvents()
        {

        }

        /// <summary>
        /// Asyncronous method used 
        /// </summary>
        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            YtChannels.AddRange(_yupRepository.ytChannels);

        }
    }
}
