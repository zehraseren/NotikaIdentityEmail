using Microsoft.EntityFrameworkCore;

namespace NotikaIdentityEmail.Context;

public class EmailContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=Zehra;initial Catalog= NotikaEmailDb;integrated security=true;trust server certificate=true");
    }
}
