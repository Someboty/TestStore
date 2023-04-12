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
        public bool IsDeleted { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CategoryAttribute: Attribute
    {
        public string Name { get; }
        public string Description { get; }
        public string Icon { get; }
        public CategoryAttribute (string name, string description, string icon)
        {
            Name = name;
            Description = description;
            Icon = icon;
        }
    }
    public enum Categories
    {
        [Category("Смартфони", "Опис смартфонів", "bi bi-phone")]
        Phones,
        [Category("Комп'ютери", "Опис комп'ютерів", "bi-pc-display")]
        Computers,
        [Category("Ноутбуки", "Опис ноутбуків", "bi bi-laptop")]
        Notebooks,
        [Category("Програмне забезпечення", "Опис програм", "bi bi-windows")]
        Programs
    }
}
