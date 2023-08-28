using DrinkVendingMachineTestApp.Models;
using DrinkVendingMachineTestApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DrinkVendingMachineTestApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<UserController> _logger;

        //Службы
        private DrinkService _drinkService;
        private CashServise _cashServise;
        private DrinkMachineServise _drinkMachineServise;

        public AdminController(ILogger<UserController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _drinkMachineServise = serviceProvider.GetService<DrinkMachineServise>();
            _drinkService = serviceProvider.GetService<DrinkService>();
            _cashServise = serviceProvider.GetService<CashServise>();
        }

        public IActionResult Index()
        {
            List<DrinkMachine> drinkMachines = _drinkMachineServise.GetDrinkMachinery();
           // DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine(1);
            //HttpContext.Session.SetInt32("DrinkMachineId", drinkMachine.Id);
            ViewData["DrinkMachines"] = drinkMachines;
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CheckDrinkMachine(int id)
        {
            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine(id);
            List<DrinkExistence> drinkExistences = drinkMachine.Drinks.ToList();
            List<CoinExistence> coinExistences = drinkMachine.Coins.ToList();
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("drinks", drinkExistences);
            result.Add("coins", coinExistences);
            return Json(result);
        }

        public IActionResult CheckUpdateRow(int idDrinkMachine, int idDrink)
        {
            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine(idDrinkMachine);
            DrinkExistence drinkExistence = _drinkService.GetDrinkExistence(drinkMachine, idDrink);
            return Json(drinkExistence);
        }

        public IActionResult SaveUpdateDrink(int idDrinkMachine, int idDrink, string drink_name, int drink_cost, int drink_count, string drink_img)
        {
            DrinkMachine drinkMachine = _drinkMachineServise.GetDrinkMachine(idDrinkMachine);
            DrinkExistence drinkExistence = _drinkService.GetDrinkExistence(drinkMachine, idDrink);
            return Json(drinkExistence);
        }

        public IActionResult SaveImg(object o)
        {
            return Json(o);
        }
    }
}