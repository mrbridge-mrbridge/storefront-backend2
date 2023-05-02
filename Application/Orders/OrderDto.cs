using Application.Purchases;
using Domain;

namespace Application.Orders
{
	public class OrderDto: OrderAbstract
    {
        public List<PurchaseDto> Purchases { get; set; }
        public string Customer { get; set; }
    }
}
