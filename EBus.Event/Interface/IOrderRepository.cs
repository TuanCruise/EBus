using System.Threading.Tasks;
using WebCore.Entities.Entities;

namespace EBus.Event
{
    public interface IOrderRepository
    {
        Task<Orders> CreateAsync(Orders toCreate);
    }
}
