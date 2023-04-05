using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public Categories Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        public byte[]? Image { get; set; }
        public List<BasketProduct> Baskets { get; set; }
    }
    public enum Categories
    {
        Phones,
        Computers,
        Notebooks,
        Programs
    }
}
