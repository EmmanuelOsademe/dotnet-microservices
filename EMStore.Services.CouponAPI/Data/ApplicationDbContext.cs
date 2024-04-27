using EMStore.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.CouponAPI.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{

		public DbSet<Coupon> Coupons { get; set; }

		// Seeding of the database
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				CouponId = 1,
				CouponCode = "10OFF",
				DiscountAmount = 10,
				MinAmount = 20
			});

			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				CouponId = 2,
				CouponCode = "20OFF",
				DiscountAmount = 20,
				MinAmount = 40
			});
		}
	}

	//public class ApplicationDbContext : DbContext
	//{
	//	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	//	{

	//	}
	//}
}
