using Microsoft.Identity.Client;

namespace DrinkVendingMachineTestApp.Models
{

    public class CoinExistence
    {
        public int Id { get; set; }
        public Coin Coin { get; set; }
        public int Count { get; set; }
        public bool Status { get; set; }
    }
}
