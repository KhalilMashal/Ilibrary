using Ilibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace Ilibrary.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

       
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
           new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
           new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
           new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );
            modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                Name = "Tech Solution",
                StreetAddress = "123 Tech St",
                City = "Tech City",
                PostalCode = "12121",
                State = "IL",
                PhoneNumber = "6669990000"
            },
new Company
{
    Id = 2,
    Name = "Vivid Books",

    StreetAddress = "999 id St",
    City = "Vid City",
    PostalCode = "66666",
    State = "IL",
    PhoneNumber = "7779990000"
},
new Company
{
    Id = 3,
    Name = "Readers Club",
    StreetAddress = "999 Main St",
    City = "Lala land",
    PostalCode = "99999",
    State = "NY",
    PhoneNumber = "1113335555"
});

        }
    }

}  

