using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace dotnet_rpg.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DbContext> options):base(options){}
        public DataContext(DbContextOptions options): base(options){}
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer(@"Server=SHREE-PC;Database=dotnet-rpg;Trusted_Connection=True;MultipleActiveResultSets=true");
        // }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        
        
        
    }
public class DataFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(@"Server=SHREE-PC;Database=dotnet-rpg;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new DataContext(optionsBuilder.Options);
    }
}


}