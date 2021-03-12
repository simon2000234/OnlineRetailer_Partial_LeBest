using System.Collections.Generic;

namespace SharedModels
{
    public class CheckPriceMessage
    {
        public decimal FundsAvailable { get; set; }

        public IList<OrderLineDTO> OrderLines { get; set; }


    }
}