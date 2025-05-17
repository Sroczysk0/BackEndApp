using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerComplaints.Core.Entities;

namespace ConsumerComplaints.Core.Interfaces
{
    public interface IComplaintRepository
    {
        Task<IEnumerable<Complaint>> GetAllAsync();
        Task<Complaint?> GetByIdAsync(int id);
        Task AddAsync(Complaint complaint);
        Task UpdateAsync(Complaint complaint);
        Task DeleteAsync(int id);
    }
}