using System.ComponentModel.DataAnnotations;

namespace DrinkVendingMachineTestApp.Models
{
    //Номинал монеты (можно добавить и купюры)
    public enum DenominatorEnum
    {
        One =1,
        Two =2,
        Five=5,
        Ten = 10
    }
    public class Coin
    {
        public int Id { get; set; }
        [Required]
        public DenominatorEnum Denominator { get; set; }
    }
}
