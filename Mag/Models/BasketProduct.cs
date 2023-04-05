using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public class BasketProduct
    {
        [Key]
        public int Id { get; set; }
        public int BasketsId { get; set; }
        public int ProductsId { get; set; }
        public Basket Baskets { get; set; }
        public Product Products { get; set; }
    }
}
