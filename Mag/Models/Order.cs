namespace Mag.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public List<OrderProduct> Products { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<StatusHistory> StatusHistories { get; set; }
        public string GetAdress => $"{State} область, місто {City}, <br /> поштовий індекс: {PostalCode}, вулиця {Street}, будинок {HouseNumber}";
        public decimal Sum => Products.Sum(o => o.Price * o.Count);
        public string StringStatus => Status switch
        {
            StatusEnum.Completed => "Завершено",
            StatusEnum.Canceled => "Відмінено",
            StatusEnum.Refunded => "Повернено",
            StatusEnum.Shipped => "Відправлено",
            StatusEnum.Declined => "Відхилено",
            StatusEnum.Accepted => "Прийнято",
            _ => "В обробці",
        };
    }
    public enum StatusEnum
    {
        Pending,
        Completed,
        Canceled,
        Refunded,
        Shipped,
        Declined,
        Accepted
    }
}
