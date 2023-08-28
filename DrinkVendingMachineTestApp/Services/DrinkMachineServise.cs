using DrinkVendingMachineTestApp.Context;
using DrinkVendingMachineTestApp.Models;

namespace DrinkVendingMachineTestApp.Services
{
    public class DrinkMachineServise
    {
        private DrinkMachineContext DrinkMachineContext { get; set; }

        public DrinkMachineServise(DrinkMachineContext drinkMachineContext)
        {
            DrinkMachineContext = drinkMachineContext;
        }

        public List<DrinkMachine> GetDrinkMachinery()
        {
            return DrinkMachineContext.DrinkMachines.ToList<DrinkMachine>();
        }

        public DrinkMachine GetDrinkMachine(int id)
        {
            List<DrinkMachine> drinkMachines = GetDrinkMachinery();
            var selects = from dm in drinkMachines where dm.Id == id select dm;
            return selects.First();
        }

        public void AddDrinkMachine(DrinkMachine drinkMachine)
        {
            DrinkMachineContext.DrinkMachines.Add(drinkMachine);
            DrinkMachineContext.SaveChanges();
        }

        public void DeleteDrinkMachine(DrinkMachine drinkMachine)
        {
            DrinkMachineContext.DrinkMachines.Remove(drinkMachine);
        }

        public void UpdateDrinkMachine()
        {

        }

    }
}
