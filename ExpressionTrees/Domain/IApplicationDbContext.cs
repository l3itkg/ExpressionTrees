using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public interface IApplicationDbContext
    {
        DbSet<Staff> Staff { get; set; }
        DbSet<StaffCategory> StaffCategory { get; set; }
    }
}