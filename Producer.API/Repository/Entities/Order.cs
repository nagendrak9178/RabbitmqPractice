using System.ComponentModel.DataAnnotations;

namespace Producer.API.Repository.Entities
{
    public record Order
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
