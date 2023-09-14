using System.ComponentModel.DataAnnotations;

namespace ProductMSystem.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Author { get; set; }
        [Required]
        public int Year { get; set; }
        public string? Description { get; set; }
        [Required]
        public int ISBN { get; set; }
    }

}
