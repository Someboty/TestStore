using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public class BasketProduct
    {
        [Key]
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public Basket Basket { get; set; }
        public Product Product { get; set; }
    }
}
