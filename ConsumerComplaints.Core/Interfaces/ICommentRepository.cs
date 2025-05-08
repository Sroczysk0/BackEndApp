using ConsumerComplaints.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerComplaints.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByComplaintIdAsync(int complaintId);
        Task AddAsync(Comment comment);
        Task SaveChangesAsync();
    }
}