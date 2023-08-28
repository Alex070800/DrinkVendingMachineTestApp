using DrinkVendingMachineTestApp.Context;
using DrinkVendingMachineTestApp.Models;
using System.Drawing;

namespace DrinkVendingMachineTestApp.Services
{
    public class CashServise
    {
        private DrinkMachineContext DrinkMachineContext { get; set; }
        private DrinkService DrinkService { get; set; }

        public CashServise(DrinkMachineContext drinkMachineContext, DrinkService drinkService)
        {
            DrinkMachineContext = drinkMachineContext;
            DrinkService = drinkService;
        }

        //Добавляет монетку в кассу, возвращаем текущую сумму в кассе
        public int AddUsersCoin(int nominal, User user, DrinkMachine drinkMachine)
        {
            if (user.PrepaidExpense.Keys.Contains((DenominatorEnum)nominal)) user.PrepaidExpense[(DenominatorEnum)nominal]++;
            else user.PrepaidExpense.Add((DenominatorEnum)nominal, 1);

            bool isContains = false;
            //Добавляем в автомат монеты
            foreach (CoinExistence coinExistence in drinkMachine.Coins)
            {
                if (coinExistence.Coin.Denominator == (DenominatorEnum)nominal)
                {
                    isContains = true;
                    coinExistence.Count++;
                }
            }
            if (!isContains)
            {
                CoinExistence coinExistence = new CoinExistence();
                Coin coin = new Coin();
                coin.Denominator = (DenominatorEnum)nominal;
                coinExistence.Coin = coin;
                coinExistence.Count = 1;
                drinkMachine.Coins.Add(coinExistence);
            }
            DrinkMachineContext.SaveChanges();

            return GetResualCashUser(user);
        }

        //Считает, сколько денег внес пользователь
        private int GetResualCashUser(User user)
        {
            int sum = 0;
            foreach (DenominatorEnum key in user.PrepaidExpense.Keys)
            {
                sum += (int)key * user.PrepaidExpense[key];
            }
            return sum;
        }

        //Сохраняет текущий выбор пользователя
        public int AddUsersDrink(int idDrink, User user)
        {
            Drink drink = DrinkMachineContext.Drinks.Find(idDrink);
            user.CheckDrink = drink;
            return drink.Price;
        }

        //Проводит покупку
        public int BuyDrink(User user, DrinkMachine drinkMachine)
        {
            if (user.CheckDrink == null) return -2;
            int cashUser = GetResualCashUser(user);
            int cashBack = 0;
            Dictionary<DenominatorEnum, int> cashBackDict = new Dictionary<DenominatorEnum, int>();
            if (cashUser >= user.CheckDrink.Price)
            {
                cashBack = cashUser - user.CheckDrink.Price;
                DrinkService.DecreaseDrinkCount(user.CheckDrink, drinkMachine);
                cashBackDict = GetCashBack(cashBack, drinkMachine, user);

            }
            else return -1;
            DrinkMachineContext.SaveChanges();
            return cashBack;
        }

        private Dictionary<DenominatorEnum, int> GetCashBack(int diff, DrinkMachine drinkMachine, User user)
        {
            //Обнуляем user. Теперь на его счету будет лежать сдача
            user.CheckDrink = null;
            user.PrepaidExpense = new Dictionary<DenominatorEnum, int>();
           // Dictionary<DenominatorEnum, int> cashBack = 
            Dictionary<DenominatorEnum, int> cashMachine = GetDictionaryCoins(drinkMachine);

            foreach (DenominatorEnum key in cashMachine.Keys)
            {
                //Если остаток от деления сдачи на монету больше 0 (можно вернуть этими монетами) и такие монеты есть
                if (diff / (int)key > 0 & cashMachine[key] > 0)
                {
                    //Пока монеты есть и сдачу можно выдавать такими монетами
                    while (cashMachine[key] > 0 && diff / (int)key > 0)
                    {
                        if (user.PrepaidExpense.Keys.Contains(key)) user.PrepaidExpense[key]++;
                        else user.PrepaidExpense.Add(key, 1);
                        cashMachine[key]--;
                        diff -= (int)key;
                    }
                    //если все вернули, выходим
                    if (diff == 0)
                    {
                        SaveDbSetDrinkMachineCash(cashMachine, drinkMachine);
                        return user.PrepaidExpense;
                    }
                }
            }
            SaveDbSetDrinkMachineCash(cashMachine, drinkMachine);
            return user.PrepaidExpense;
        }


        private void SaveDbSetDrinkMachineCash(Dictionary<DenominatorEnum, int> cashMachine, DrinkMachine drinkMachine)
        {
            foreach (CoinExistence coinExistence in drinkMachine.Coins)
            {
                coinExistence.Count = cashMachine[coinExistence.Coin.Denominator];
            }
            DrinkMachineContext.SaveChanges();
        }

        private Dictionary<DenominatorEnum, int> GetDictionaryCoins(DrinkMachine drinkMachine)
        {
            Dictionary<DenominatorEnum, int> coins = new Dictionary<DenominatorEnum, int>();
            foreach (CoinExistence coinExistence in drinkMachine.Coins)
            {
                coins.Add(coinExistence.Coin.Denominator, coinExistence.Count);
            }
            return coins;
        }
    }
}
