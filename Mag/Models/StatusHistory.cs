namespace Mag.Models
{
    public class StatusHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime StatusChanged { get; set; }
    }
}
