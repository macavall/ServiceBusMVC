using System.Threading.Tasks;

namespace ServiceBusMVC
{
    public interface ISbService
    {
        public Task<int> GetQueueMsgCount();
    }
}