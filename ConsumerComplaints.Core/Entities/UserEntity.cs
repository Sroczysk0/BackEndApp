using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;

namespace ConsumerComplaints.Infrastructure.Entities;


public class UserEntity: IdentityUser
{
    public UserDetails Details { get; set; } 
}