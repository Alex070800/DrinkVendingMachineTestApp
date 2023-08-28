using DrinkVendingMachineTestApp.Context;
using DrinkVendingMachineTestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachineTestApp.Services
{
    public class DrinkService
    {
        private DrinkMachineContext DrinkMachineContext { get; set; }

        public DrinkService(DrinkMachineContext drinkMachineContext)
        {
            DrinkMachineContext = drinkMachineContext;
        }

        public void AddDrink(Drink drink, DrinkMachine drinkMachine)
        {
            DrinkExistence drinkExistence = new DrinkExistence();
            drinkExistence.Drink = drink;
            drinkExistence.Count = 0;
            drinkMachine.Drinks.Add(drinkExistence);
            //DrinkMachineContext.DrinkMachines.Find(drinkMachine.Id).Drinks.Add(drinkExistence);
            DrinkMachineContext.SaveChanges();
        }

        public void DeleteDrink(Drink drink)
        {
            DrinkMachineContext.Drinks.Remove(drink);
            DrinkMachineContext.SaveChanges();
        }

        public void UpdateDrink()
        {

        }

        public List<Drink> GetDrinks(DrinkMachine drinkMachine)
        {
             List<Drink> drinks = new List<Drink>();
            //Выбираем напитки, которые есть только в рассматриваемой машине с напитками.
            foreach(DrinkExistence drinkExistence in drinkMachine.Drinks)
            {
                if(drinkExistence.Count!=0) drinks.Add(drinkExistence.Drink);
            }
            return drinks;
        }

        public DrinkExistence GetDrinkExistence(DrinkMachine drinkMachine, int drinkId)
        {
            foreach(DrinkExistence drinkExistence in drinkMachine.Drinks)
            {
                if(drinkExistence.Drink.Id == drinkId)
                {
                    return drinkExistence;
                }
            }
            return null;
        }

        public void DecreaseDrinkCount(Drink drink, DrinkMachine drinkMachine)
        {
            var selecets = from d in drinkMachine.Drinks where d.Drink == drink select d;
            var drinkExist = selecets.First();
            if(drinkExist.Count>=0) drinkExist.Count--;
            DrinkMachineContext.SaveChanges();
        }
    }
}
