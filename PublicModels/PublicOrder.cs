using System;

namespace PublicModels
{
    public class PublicOrder
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public StatusEnum Status { get; set; }


        public enum StatusEnum
        {
            Completed,
            Canceled,
            Shipped,
            Paid,
        }
    }
}
