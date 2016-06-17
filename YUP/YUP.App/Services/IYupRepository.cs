using System.Collections.Generic;
using YUP.App.Models;

namespace YUP.App.Services
{
    public interface IYupRepository
    {
        void LoadRepository();
        void SaveRepository();

        void AddChannel(YTChannel channel);
        void Editchannel(YTChannel channel);
        void RemoveChannel(YTChannel channel);
        List<YTChannel> GetAllYtChannels(); 

        void AddYupi(YupItem yupi);
        void EditYupi(YupItem yupi);
        void RemoveYupi(YupItem yupi);

        void AddCategory(string cat);
        List<string> GetAllCategories();
        void EditCategory(string cat, string newCat);
        void RemoveCategory(string cat);

    }
}
