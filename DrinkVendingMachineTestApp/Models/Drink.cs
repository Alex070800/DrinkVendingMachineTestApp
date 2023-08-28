using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkVendingMachineTestApp.Models
{
    public class Drink
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }  
    }
}
