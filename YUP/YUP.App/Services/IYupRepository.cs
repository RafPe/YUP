using System.Collections.Generic;
using YUP.App.Models;

namespace YUP.App.Services
{
    public interface IYupRepository
    {
        IEnumerable<YTVideo>    ytVideos            { get; set; }
        IEnumerable<YupItem>    yupItems            { get; set; } 
        IEnumerable<YTChannel>  ytChannels          { get; set; } 

        YTChannel               currentlySelected   { get; set; }

        void LoadRepository();
        void SaveRepository();

        void AddChannel();
        void Editchannel();
        void RemoveChannel();

        void LoadYupis();
        void AddYupi();
        void EditYupi();
        void RemoveYupi();

    }
}
