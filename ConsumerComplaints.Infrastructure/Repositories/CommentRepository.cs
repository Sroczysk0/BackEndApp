using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerComplaints.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByComplaintIdAsync(int complaintId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.ComplaintId == complaintId)
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}