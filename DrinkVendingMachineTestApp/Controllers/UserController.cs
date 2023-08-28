using DrinkVendingMachineTestApp.Context;
using DrinkVendingMachineTestApp.Models;
using DrinkVendingMachineTestApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DrinkVendingMachineTestApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        //Службы
        private DrinkService _drinkService;
        private CashServise _cashServise;
        private DrinkMachineServise _drinkMachineServise;
        private static BlockingCollection<User> Users { get; set; } = new BlockingCollection<User>();

        public UserController(ILogger<UserController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _drinkMachineServise = serviceProvider.GetService<DrinkMachineServise>();
            _drinkService = serviceProvider.GetService<DrinkService>();
            _cashServise = serviceProvider.GetService<CashServise>();
        }


        [HttpGet]
        public IActionResult Index(string adminKey, int idDrinkMachine=1)
        {

            //Предполагается, что автоматы стоят по всему городу)
            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine(idDrinkMachine);
            HttpContext.Session.SetInt32("DrinkMachineId", drinkMachine.Id);

            //Добавляем новую сессию
            User user = new User(Users.Count + 1);
            Users.Add(user);
            HttpContext.Session.SetInt32("UserId", user.Id);

            if (string.IsNullOrEmpty(adminKey))
            {
                List<Drink> drinks = _drinkService.GetDrinks(drinkMachine);
                ViewData["Drinks"] = drinks;
                return View();
            }
            else return RedirectToAction("Index", "Admin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddUsersCoin(int nominal)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine((int)HttpContext.Session.GetInt32("DrinkMachineId"));
            int sum = _cashServise.AddUsersCoin(nominal, GetUserById(userId), drinkMachine);
            return Json(sum);
        }

        private User GetUserById(int id)
        {
            var users = from u in Users where u.Id == id select u;
            return users.First();
        }

        [HttpPost]
        public IActionResult AddUsersDrink(int id)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            int sum = _cashServise.AddUsersDrink(id, GetUserById(userId));
            return Json(sum);
        }

        public IActionResult BuyDrink()
        {
            User user = GetUserById((int)HttpContext.Session.GetInt32("UserId"));

            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine((int)HttpContext.Session.GetInt32("DrinkMachineId"));

            int cashBack = _cashServise.BuyDrink(user, drinkMachine);

            return Json(cashBack);
        }

        public IActionResult ReturnCashBack()
        {
            User user = GetUserById((int)HttpContext.Session.GetInt32("UserId"));
            var result = user.PrepaidExpense;
            
            user.PrepaidExpense = new Dictionary<DenominatorEnum, int>();
            if (result.Count == 0) return Json(-1);
            else return Json(result);
        }
    }
}