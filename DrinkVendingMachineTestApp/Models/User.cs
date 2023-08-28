namespace DrinkVendingMachineTestApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public Dictionary<DenominatorEnum, int> PrepaidExpense { get; set; } // Деньги, внесенные пользователем 
        public Drink CheckDrink { get; set; } //Выбранный напиток

        public User(int id) 
        {
            Id=id;
            PrepaidExpense = new Dictionary<DenominatorEnum, int>();
        }

    }
}
