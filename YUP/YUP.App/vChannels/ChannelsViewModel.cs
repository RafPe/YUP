using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Google.Apis.YouTube.v3.Data;
using MaterialDesignThemes.Wpf;
using YUP.App.Base;
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
        #region Interfaces used 
        private IYupRepository  _yupRepository;
        private IYtManager      _ytManager;
        private IEventBus       _eventBus;
        private IHoldingBay     _holdingbay;
        #endregion

        #region Channels and selected channel
        public ObservableCollection<YTChannel>  YtChannels          { get; set; }
        public YTChannel                        SelectedYtChannel
        {
            get { return _selectedYtChannel; }
            set { _selectedYtChannel = value; }
        }
        #endregion

        #region EventOnBus
        public event EventBusHandler channelAdded;
        public event EventBusHandler channelRemoved;
        #endregion

        #region RelayCommands
        public RelayCommand CardShareCmd            { get; private set; }
        public RelayCommand CardDeleteCmd           { get; private set; }
        public RelayCommand CardEditCmd             { get; private set; }
        public RelayCommand CardFavoriteCmd         { get; private set; }
        public RelayCommand SearchBoxCmd            { get; private set; }
        #endregion

        #region CollectionViewSource
        internal CollectionViewSource CvsStaff { get; set; }
        public ICollectionView AllStaff
        {
            get { return CvsStaff.View; }
        }
        #endregion

        #region String properties
        // Binding on textbox using http://stackoverflow.com/a/20089930/2476347
        private string _searchBoxTerm;
        public string SearchBoxTerm
        {
            get { return this._searchBoxTerm; }
            set
            {
                this._searchBoxTerm = value;
                if (SearchBoxTerm.Length == 0 || SearchBoxTerm.Length > 3) OnFilterChanged();
            }
        }
        #endregion

        #region private props

        private YTChannel _selectedYtChannel;
        private bool      isDataLoaded = false;

        #endregion

        #region Default ctor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="yupRepository"></param>
        /// <param name="ytManager"></param>
        /// <param name="eventbus"></param>
        public ChannelsViewModel(IYupRepository yupRepository, IYtManager ytManager, IEventBus eventbus,IHoldingBay holdingbay)
        {
            _yupRepository  = yupRepository;
            _ytManager      = ytManager;
            _eventBus       = eventbus;
            _holdingbay     = holdingbay;

            YtChannels      = new ObservableCollection<YTChannel>();


            CvsStaff        = new CollectionViewSource();
            CvsStaff.Source = this.YtChannels;
            CvsStaff.Filter += FilterChannels;

            CardShareCmd    = new RelayCommand(OnCardShareCmd);
            CardDeleteCmd   = new RelayCommand(OnCardDeleteCmd);
            CardEditCmd     = new RelayCommand(OnCardEdited);
            CardFavoriteCmd = new RelayCommand(OnCardFavoriteCmd);

            SearchBoxCmd    = new RelayCommand(OnSearchBoxCmd);


        }
        #endregion


        #region RelayCommands handlers

        /// <summary>
        /// Handler executed when SearchBoxCmd command is invoked
        /// </summary>
        private async void OnSearchBoxCmd()
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
                thumbnail   = tmpChannelStats.Snippet.Thumbnails.High.Url,
                channelId   = tmpChannelStats.Id,
                user        = tmpChannelStats.Snippet.Title
            };

            /* 
                At this stage we have channel so we can push it into our holding bay.
                Probably there is a better way to do it - just havent discovered it yet :) 
            */
            _holdingbay.AddEntry(HoldingBayItem.CHANNEL_NEW, chann);

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DialogEditChannel()
            {
                DataContext = new DialogEditChannelViewModel()
            };

            //show the dialog
            bool result = (bool)await DialogHost.Show(view, DialogHostId.MAINWiNDOW_ROOT);

            if (!result) return;    // User cliked cancel - so we dont add this channel

            chann = (YTChannel) _holdingbay.GetEntry(HoldingBayItem.CHANNEL_NEW);

            _yupRepository.AddChannel(chann);
            _yupRepository.SaveRepository();

            YtChannels.Add(chann);

            _eventBus.RaiseEvent(EventOnBus.channelAdded, this, new EventBusArgs() { Item = chann });

            // Clean up searchbox terms 
            _searchBoxTerm = "";
            SearchBoxTerm = "";
            this.OnFilterChanged();
              
        }

        /// <summary>
        /// Handler executed when CardFavoriteCmd command is invoked
        /// </summary>
        private void OnCardFavoriteCmd()
        {

            if (!ReferenceEquals(_selectedYtChannel, null))
            {
                _selectedYtChannel.isFavorite = !_selectedYtChannel.isFavorite;
            }

        }

        /// <summary>
        /// Handler executed when CardEditmd command is invoked
        /// </summary>
        private async void OnCardEdited()
        {

            if (ReferenceEquals(_selectedYtChannel, null)) return;

            // Create a copy if we would cancel our edit 
            var channelBackup = Activator.CreateInstance<YTChannel>();
            var fields = channelBackup.GetType().GetFields(BindingFlags.Public
                | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(_selectedYtChannel);
                field.SetValue(channelBackup, value);
            }

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DialogEditChannel()
            {
                DataContext = new DialogEditChannelViewModel(_selectedYtChannel)
            };

            //show the dialog
            bool result = (bool)await DialogHost.Show(view, DialogHostId.MAINWiNDOW_ROOT);

            // User cliked cancel - so we dont edit this channel
            if (!result)
            {
                _selectedYtChannel = channelBackup;
                return;                             
            }

            // Save change in our repository
            _yupRepository.SaveRepository();

            // Fire up event that we had a channel modification
            _eventBus.RaiseEvent(EventOnBus.channelEdited, this, new EventBusArgs() { Item = _selectedYtChannel });

            // Clean up searchbox terms 
            _searchBoxTerm = "";
            SearchBoxTerm = "";

        }

        /// <summary>
        /// Handler executed when CardDeleteCmd command is invoked
        /// </summary>
        private async void OnCardDeleteCmd()
        {

            if (_selectedYtChannel == null) return;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DialogRemoveChannel()
            {
                DataContext = this
            };

            //show the dialog
            bool result = (bool) await DialogHost.Show(view, DialogHostId.MAINWiNDOW_ROOT);

            if (result)
            {
                YtChannels.Remove(_selectedYtChannel);                   // Remove videos from our colection

                this.OnFilterChanged();                                 // Refresh collection source view

                AllStaff.Refresh();

                _yupRepository.RemoveChannel(_selectedYtChannel);   // Remove channel from repository
                _yupRepository.SaveRepository();                        // Save repo

                _eventBus.RaiseEvent(EventOnBus.channelRemoved,         // Raie event to notify the remaining components
                                     this, new EventBusArgs()
                                     {
                                         Item = _selectedYtChannel
                                     });

            }

        }

        /// <summary>
        /// Handler executed when CardShareCmd command is invoked
        /// </summary>
        private void OnCardShareCmd()
        {


        }

        #endregion

        #region EventsOnBus Publication/Subscription

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

        #endregion

        #region Async OnLoaded

        /// <summary>
        /// Async method used for loading when component is ready
        /// </summary>
        public async void LoadData()
        {

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()) || isDataLoaded) return;

            isDataLoaded = true;

            YtChannels.AddRange(_yupRepository.GetAllYtChannels());

        }

        #endregion

        #region Filtering channels

        /// <summary>
        /// Method responsible for refreshing view
        /// </summary>
        private void OnFilterChanged()
        {
            CvsStaff.View.Refresh();
        }

        /// <summary>
        /// Method responsible for filtering based on username 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterChannels(object sender, FilterEventArgs e)
        {
            YTChannel svm = (YTChannel)e.Item;

            if (string.IsNullOrWhiteSpace(this.SearchBoxTerm) || this.SearchBoxTerm.Length == 0 || this.SearchBoxTerm.Length < 3)
            {
                e.Accepted = true;
            }
            else
            {
                if (!ReferenceEquals(svm.user, null))
                {
                    e.Accepted = (svm.friendlyName.IndexOf(SearchBoxTerm, StringComparison.OrdinalIgnoreCase)) >= 0  || (svm.user.IndexOf(SearchBoxTerm, StringComparison.OrdinalIgnoreCase)) >= 0 || (svm.description.IndexOf(SearchBoxTerm, StringComparison.OrdinalIgnoreCase)) >= 0;
                }
                else
                {
                    e.Accepted = true;
                }

            }
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Simple method to check if input is URL
        /// using regex
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns></returns>
        private bool IsUrl(string input)
        {

            var r = new Regex(@"^http(s)?.*");

            var match = r.Match(input);

            if (match.Success) return true;

            return false;

        }

        #endregion

    }
}
