using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;

namespace ConsumerComplaints.Core.Entities;

public class UserEntity : IdentityUser
{
    public UserDetails Details { get; set; } = null!;
}