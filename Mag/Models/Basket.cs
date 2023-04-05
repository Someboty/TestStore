using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public AspNetUser? User { get; set; }
        public List<BasketProduct> Products { get; set; }

    }
}
