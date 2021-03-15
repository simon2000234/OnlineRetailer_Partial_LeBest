using System.Collections.Generic;

namespace SharedModels
{
    public class CheckPriceMessage
    {
        public IList<OrderLineDTO> OrderLines { get; set; }


    }
}