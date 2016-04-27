using System.Collections.Generic;
using YUP.App.Models;

namespace YUP.App.Services
{
    public interface IYupRepository
    {
        List<YTVideo>    ytVideos            { get; set; }
        List<YupItem>    yupItems            { get; set; }
        List<YTChannel>  ytChannels          { get; set; } 

        YTChannel               currentlySelected   { get; set; }

        void LoadRepository();
        void SaveRepository();

        void AddChannel(YTChannel channel);
        void Editchannel(YTChannel channel);
        void RemoveChannel(YTChannel channel);

        void AddYupi(YupItem yupi);
        void EditYupi(YupItem yupi);
        void RemoveYupi(YupItem yupi);

    }
}
