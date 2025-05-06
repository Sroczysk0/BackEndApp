// using ConsumerComplaints.Core.Entities;
// using ConsumerComplaints.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;
//
// namespace ConsumerComplaints.Infrastructure.Seed
//
// {
//
//     public static class CsvSeeder
//
//     {
//
//         public static async Task SeedFromCsvAsync(AppDbContext context, string filePath)
//
//         {
//
//             if (!File.Exists(filePath))
//
//             {
//
//                 Console.WriteLine($"Plik CSV nie został znaleziony: {filePath}");
//
//                 return;
//
//             }
//  
//             if (await context.Complaints.AnyAsync()) return; // Jeśli dane już są, nie ładujemy drugi raz
//  
//             var lines = await File.ReadAllLinesAsync(filePath);
//
//             var complaints = new List<Complaint>();
//  
//             foreach (var line in lines.Skip(1)) // pomiń nagłówek CSV
//
//             {
//
//                 var parts = line.Split(';');
//  
//                 if (parts.Length < 4) continue;
//  
//                 complaints.Add(new Complaint
//
//                 {
//
//                     Product = parts[0],
//
//                     Issue = parts[1],
//
//                     Company = parts[2],
//
//                     DateReceived = DateTime.TryParse(parts[3], out var date)
//
//                         ? date
//
//                         : DateTime.MinValue
//
//                 });
//
//             }
//  
//             await context.Complaints.AddRangeAsync(complaints);
//
//             await context.SaveChangesAsync();
//
//         }
//
//     }
//
// }