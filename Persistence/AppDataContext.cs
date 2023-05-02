using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence;
public class AppDataContext: DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options): base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<CustomerReview> Reviews { get; set; }
    public DbSet<ReviewReply> ReviewReplies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<ShippingDetails> ShipingDetails { get; set; }
    public DbSet<CreditCardDetail> CreditCardDetails { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<PagePhoto> PagePhotos { get; set; }
	public DbSet<Discount> Discounts { get; set; }
	public DbSet<Charge> Charges { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Customer>();
        modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        modelBuilder.Entity<Store>( s => 
        {
            s.HasKey(s => s.StoreId);

            s.HasOne(s => s.Merchant)
            .WithMany(m => m.Stores)
            .HasForeignKey(s => s.MerchantId);

            s.HasMany(s => s.Inventory)
            .WithOne(p => p.Store)
            .HasForeignKey(p => p.StoreId);

        });


        modelBuilder.Entity<Purchase>( 
            entity =>
            {
            entity.HasKey(d => new { d.OrderId, d.ProductId });

            entity.HasOne(d => d.Product)
            .WithMany(p => p.Purchases)
            .HasForeignKey(o => o.ProductId);
			}
        );

		modelBuilder
		.Entity<Order>()
		.Property(d => d.OrderState)
		.HasConversion(new EnumToStringConverter<OrderStates>());

		modelBuilder.Entity<CustomerReview>(
			entity =>
			{
                entity.HasKey(r => new { r.ProductId, r.CustomerId});

                entity.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

                entity.HasOne(r => r.Customer)
                .WithMany(c => c.ProductReviews)
                .HasForeignKey(r => r.CustomerId);
            }
        );

        modelBuilder.Entity<ReviewReply>(
            entity => {
                entity.HasKey(rr => rr.MerchantId);
                
                entity.HasOne(rr => rr.Merchant)
                .WithMany(r => r.ReviewReplies)
                .HasForeignKey( rr => rr.MerchantId);

				entity.HasOne(rr => rr.Review)
				.WithOne(r => r.ReviewReply)
				.HasForeignKey<ReviewReply>(rr => new {rr.CustomerId, rr.ProductId});
			}
        );

        modelBuilder.Entity<CreditCardDetail>(entity =>
        {
            entity.HasKey(s => new { s.StoreId, s.CustomerId });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_cace4a159ff9f2512dd42373760");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ_e12875dfb3b1d92d7d7c5377e22").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Activated).HasDefaultValueSql("false").HasColumnName("activated");
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.OauthId)
                .HasColumnType("character varying")
                .HasColumnName("oauth_id");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");

            entity.HasDiscriminator(e => e.Role)
                .HasValue<Customer>("customer")
                .HasValue<Merchant>("business");
            
        });

        modelBuilder.Entity<Merchant>(entity =>
            {
                entity.Property(e => e.BusinessName)
                .HasColumnType("character varying")
                .HasColumnName("business_name");
            });

		modelBuilder.Entity<Photo>(entity =>
            {
				entity.HasDiscriminator(p => p.CategoryName);

			});

        modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("orderId");

            });

		modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_82fae97f905930df5d62a702fc9");

            entity.ToTable("token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivationCode)
                .HasColumnType("character varying")
                .HasColumnName("activation_code");
            entity.Property(e => e.Token1)
                .HasColumnType("character varying")
                .HasColumnName("token");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");
        });

	}


}
