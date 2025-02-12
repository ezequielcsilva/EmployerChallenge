using Employer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Employer.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
