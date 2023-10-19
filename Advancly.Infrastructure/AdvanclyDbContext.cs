using Advancly.Domain.Entitities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Advancly.Infrastructure
{
    public class AdvanclyDbContext : IdentityDbContext<IdentityUser>
    {
        #region ctor
        public AdvanclyDbContext([NotNull] DbContextOptions<AdvanclyDbContext> options) : base(options)
        {             
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AdvanclyUser>().HasBaseType<IdentityUser>();
            builder.Entity<Transaction>().Property(v => v.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate().HasDefaultValue(1);
        }


        public DbSet<AdvanclyUser> AdvanclyUsers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
