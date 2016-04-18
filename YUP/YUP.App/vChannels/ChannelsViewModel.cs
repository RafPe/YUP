using System.Collections.ObjectModel;
using YUP.App.Helpers;
using YUP.App.Models;
using YUP.App.Services;

namespace YUP.App.vChannels
{
    public class ChannelsViewModel : BindableBase
    {
        private IYupRepository _yupRepository;

        public ObservableCollection<string> test { get; set; }
        public ObservableCollection<YTChannel> YtChannels { get; set; }

        public string testos { get;set; } = "xyz";



        public ChannelsViewModel(IYupRepository yupRepository)
        {
            _yupRepository = yupRepository;

            YtChannels = new ObservableCollection<YTChannel>();

            YtChannels.AddRange(_yupRepository.ytChannels);

        }
    }
}
