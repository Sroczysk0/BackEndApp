using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConsumerComplaints.Infrastructure.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _context;

        public ComplaintRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Complaint>> GetAllAsync()
        {
            return await _context.Complaints.ToListAsync();
        }

        public async Task<Complaint?> GetByIdAsync(int id)
        {
            return await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Complaint complaint)
        {
            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Complaint complaint)
        {
            _context.Complaints.Update(complaint);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var complaint = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == id);
            if (complaint == null)
            {
                Console.WriteLine($"Complaint o ID {id} nie istnieje");
                return;
            }

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Usunięto complaint o ID {id}");
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}