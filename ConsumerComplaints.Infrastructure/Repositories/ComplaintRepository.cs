using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Infrastructure.Persistence;
using CustomerComplaints.Core.Interfaces;
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

        Task<Complaint> IComplaintRepository.GetByIdAsync(int id)
        {
            return GetByIdAsync(id);
        }

        Task IComplaintRepository.AddAsync(Complaint complaint)
        {
            return AddAsync(complaint);
        }

        Task IComplaintRepository.UpdateAsync(Complaint complaint)
        {
            return UpdateAsync(complaint);
        }

        Task<IEnumerable<Complaint>> IComplaintRepository.GetAllAsync()
        {
            return GetAllAsync();
        }

        public async Task<Complaint> GetByIdAsync(int id)
        {
            return await _context.Complaints.FindAsync(id);
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
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
            }
        }
    }
}