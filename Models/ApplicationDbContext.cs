using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOT_TASK.Models
{
    public class ApplicationDbContext : IdentityDbContext<UserModel, RoleModel, Guid>
    {
        public DbSet<UserModel> Users {  get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<FlightModel> Flights { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }*/

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedAt = now;
                }

                ((BaseModel)entity.Entity).ModifiedAt = now;
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedAt = now;
                }

                ((BaseModel)entity.Entity).ModifiedAt = now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
