﻿namespace Mag.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Price { get; set; }
        public decimal Sum => Count * Price;
    }
}
