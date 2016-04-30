using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Services
{
    /// <summary>
    /// Interface exposing methods used for registrations/publications
    /// </summary>
    public interface IEventRegistrator
    {
        void PublishEvents();
        void SubscribeEvents();

    }
}
