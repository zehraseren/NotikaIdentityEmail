using NotikaIdentityEmail.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NotikaIdentityEmail.Context;

public class EmailContext : IdentityDbContext<AppUser>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=Zehra;initial Catalog= NotikaEmailDb;integrated security=true;trust server certificate=true");
    }
}
