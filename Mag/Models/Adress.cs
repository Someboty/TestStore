using System.IO;

namespace Mag.Models
{
    public class Adress
    {
        public int Id { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public bool IsPrimary { get; set; } = false;
        public string? UserId { get; set; }
        public AspNetUser? User { get; set; }
        public string GetAdress => $"{State} область, місто {City}, <br /> поштовий індекс: {PostalCode}, вулиця {Street}, будинок {HouseNumber}";
    }
}
