using ConsumerComplaints.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerComplaints.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByComplaintIdAsync(int complaintId);
        Task<Comment?> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}