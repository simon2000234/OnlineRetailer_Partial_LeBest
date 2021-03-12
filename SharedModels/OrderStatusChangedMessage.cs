using System.Collections.Generic;

namespace SharedModels
{
    public class OrderStatusChangedMessage
    {
        public int? CustomerId { get; set; }
        public IList<OrderLineDTO> OrderLines { get; set; }
    }
}
