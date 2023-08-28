namespace DrinkVendingMachineTestApp.Models
{
    public class DrinkMachine
    {
        public int Id { get; set; }
        public List<DrinkExistence> Drinks { get; set; } = new List<DrinkExistence>();
        public List<CoinExistence> Coins { get; set; } = new List<CoinExistence> { };
        public string Address { get; set; }
    }
}
