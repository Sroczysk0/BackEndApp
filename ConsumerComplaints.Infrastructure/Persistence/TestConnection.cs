using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConsumerComplaints.Infrastructure.Persistence;

namespace TestDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=C:/data/consumer_complaints.db"))
                .BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<AppDbContext>();

            var tableExists = context.Database.ExecuteSqlRaw("SELECT 1 FROM complaints LIMIT 1");
            Console.WriteLine($"Tabela 'complaints' działa. Wynik: {tableExists}");
        }
    }
}