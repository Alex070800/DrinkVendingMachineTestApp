using DrinkVendingMachineTestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachineTestApp.Context
{
    public class DrinkMachineContext : DbContext
    {
        public DbSet<Drink> Drinks => Set<Drink>();

        public DbSet<DrinkExistence> DrinkExistences => Set<DrinkExistence>();

        public DbSet<Coin> Coin => Set<Coin>();

        public DbSet<CoinExistence> CoinExistences => Set<CoinExistence>();

        public DbSet<DrinkMachine> DrinkMachines => Set<DrinkMachine>();

        public DrinkMachineContext()
        {
            Database.EnsureCreated();
            
            Drinks.Load<Drink>();
            DrinkExistences.Load<DrinkExistence>();
            Coin.Load<Coin>(); 
            CoinExistences.Load<CoinExistence>();
            DrinkMachines.Load<DrinkMachine>(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DrinkMachineDB;TrustServerCertificate=True; Trusted_Connection=True;");
        }

       
    }
}
