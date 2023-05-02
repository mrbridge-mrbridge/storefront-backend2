﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20230411100415_Quantities-To-Deci")]
    partial class QuantitiesToDeci
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Charge", b =>
                {
                    b.Property<string>("ChargeId")
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.HasKey("ChargeId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("Domain.CreditCardDetail", b =>
                {
                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Method")
                        .HasColumnType("text");

                    b.Property<string>("StripeId")
                        .HasColumnType("text");

                    b.HasKey("StoreId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CreditCardDetails");
                });

            modelBuilder.Entity("Domain.CustomerReview", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCommented")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Domain.Discount", b =>
                {
                    b.Property<Guid>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Rate")
                        .HasColumnType("numeric");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.HasKey("DiscountId");

                    b.HasIndex("StoreId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOrdered")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OrderState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ShippingDetailsId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Page", b =>
                {
                    b.Property<Guid>("PageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("FacebookLink")
                        .HasColumnType("text");

                    b.Property<string>("FooterText")
                        .HasColumnType("text");

                    b.Property<string>("Heading")
                        .HasColumnType("text");

                    b.Property<string>("HeroImage")
                        .HasColumnType("text");

                    b.Property<string>("InstagramLink")
                        .HasColumnType("text");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("MainColor")
                        .HasColumnType("text");

                    b.Property<string>("MainHeaderTextSize")
                        .HasColumnType("text");

                    b.Property<string>("PageCategory")
                        .HasColumnType("text");

                    b.Property<int>("PageNumber")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("Publish")
                        .HasColumnType("boolean");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<string>("SubColor")
                        .HasColumnType("text");

                    b.Property<string>("SubHeaderTextsize")
                        .HasColumnType("text");

                    b.Property<string>("SubHeading")
                        .HasColumnType("text");

                    b.Property<string>("TwitterLink")
                        .HasColumnType("text");

                    b.HasKey("PageId");

                    b.HasIndex("StoreId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Domain.Photo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Photos");

                    b.HasDiscriminator<string>("CategoryName").HasValue("Photo");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DiscountId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProductCategory")
                        .HasColumnType("text");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("text");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<bool>("Publish")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<string>("UnitOfMeasurement")
                        .HasColumnType("text");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Domain.Purchase", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("Discount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("QuantityPurchased")
                        .HasColumnType("numeric");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Domain.ReviewReply", b =>
                {
                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateReplied")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("Reply")
                        .HasColumnType("text");

                    b.HasKey("MerchantId");

                    b.HasIndex("CustomerId", "ProductId")
                        .IsUnique();

                    b.ToTable("ReviewReplies");
                });

            modelBuilder.Entity("Domain.ShippingDetails", b =>
                {
                    b.Property<Guid>("ShippingDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<string>("StreetName")
                        .HasColumnType("text");

                    b.HasKey("ShippingDetailsId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StoreId");

                    b.ToTable("ShipingDetails");
                });

            modelBuilder.Entity("Domain.Store", b =>
                {
                    b.Property<Guid>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<string>("CurrencySymbol")
                        .HasColumnType("text");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("integer");

                    b.Property<string>("StoreName")
                        .HasColumnType("text");

                    b.HasKey("StoreId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Domain.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ActivationCode")
                        .HasColumnType("character varying")
                        .HasColumnName("activation_code");

                    b.Property<string>("Token1")
                        .HasColumnType("character varying")
                        .HasColumnName("token");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_82fae97f905930df5d62a702fc9");

                    b.ToTable("token", (string)null);
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("Activated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasColumnName("activated")
                        .HasDefaultValueSql("false");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .HasColumnType("character varying")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("character varying")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("character varying")
                        .HasColumnName("last_name");

                    b.Property<string>("OauthId")
                        .HasColumnType("character varying")
                        .HasColumnName("oauth_id");

                    b.Property<string>("Password")
                        .HasColumnType("character varying")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("character varying")
                        .HasColumnName("phone_number");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("PK_cace4a159ff9f2512dd42373760");

                    b.HasIndex(new[] { "Email" }, "UQ_e12875dfb3b1d92d7d7c5377e22")
                        .IsUnique();

                    b.ToTable("user", (string)null);

                    b.HasDiscriminator<string>("Role").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.PagePhoto", b =>
                {
                    b.HasBaseType("Domain.Photo");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uuid");

                    b.HasIndex("PageId");

                    b.HasDiscriminator().HasValue("PagePhoto");
                });

            modelBuilder.Entity("Domain.ProductPhoto", b =>
                {
                    b.HasBaseType("Domain.Photo");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.HasIndex("ProductId");

                    b.HasDiscriminator().HasValue("ProductPhoto");
                });

            modelBuilder.Entity("Domain.Customer", b =>
                {
                    b.HasBaseType("Domain.User");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("orderId");

                    b.HasDiscriminator().HasValue("customer");
                });

            modelBuilder.Entity("Domain.Merchant", b =>
                {
                    b.HasBaseType("Domain.User");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("BusinessName")
                        .HasColumnType("character varying")
                        .HasColumnName("business_name");

                    b.Property<string>("StreetName")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("business");
                });

            modelBuilder.Entity("Domain.Charge", b =>
                {
                    b.HasOne("Domain.Order", "Order")
                        .WithOne("Charge")
                        .HasForeignKey("Domain.Charge", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Domain.CreditCardDetail", b =>
                {
                    b.HasOne("Domain.Customer", "Customer")
                        .WithMany("CreditCardDetails")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Store", "Store")
                        .WithMany("CreditCards")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.CustomerReview", b =>
                {
                    b.HasOne("Domain.Customer", "Customer")
                        .WithMany("ProductReviews")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Discount", b =>
                {
                    b.HasOne("Domain.Store", "Store")
                        .WithMany("Discounts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.HasOne("Domain.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Page", b =>
                {
                    b.HasOne("Domain.Store", "Store")
                        .WithMany("Pages")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.HasOne("Domain.Store", "Store")
                        .WithMany("Inventory")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.Purchase", b =>
                {
                    b.HasOne("Domain.Order", "Order")
                        .WithMany("Purchases")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Product", "Product")
                        .WithMany("Purchases")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.ReviewReply", b =>
                {
                    b.HasOne("Domain.Merchant", "Merchant")
                        .WithMany("ReviewReplies")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.CustomerReview", "Review")
                        .WithOne("ReviewReply")
                        .HasForeignKey("Domain.ReviewReply", "CustomerId", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Merchant");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("Domain.ShippingDetails", b =>
                {
                    b.HasOne("Domain.Customer", "Customer")
                        .WithMany("ShipingDetails")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Store", "Store")
                        .WithMany("ShipingDetails")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.Store", b =>
                {
                    b.HasOne("Domain.Merchant", "Merchant")
                        .WithMany("Stores")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("Domain.PagePhoto", b =>
                {
                    b.HasOne("Domain.Page", "Page")
                        .WithMany("PagePhotos")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("Domain.ProductPhoto", b =>
                {
                    b.HasOne("Domain.Product", "Product")
                        .WithMany("ProductPhotos")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.CustomerReview", b =>
                {
                    b.Navigation("ReviewReply");
                });

            modelBuilder.Entity("Domain.Order", b =>
                {
                    b.Navigation("Charge");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("Domain.Page", b =>
                {
                    b.Navigation("PagePhotos");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.Navigation("ProductPhotos");

                    b.Navigation("Purchases");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Domain.Store", b =>
                {
                    b.Navigation("CreditCards");

                    b.Navigation("Discounts");

                    b.Navigation("Inventory");

                    b.Navigation("Pages");

                    b.Navigation("ShipingDetails");
                });

            modelBuilder.Entity("Domain.Customer", b =>
                {
                    b.Navigation("CreditCardDetails");

                    b.Navigation("Orders");

                    b.Navigation("ProductReviews");

                    b.Navigation("ShipingDetails");
                });

            modelBuilder.Entity("Domain.Merchant", b =>
                {
                    b.Navigation("ReviewReplies");

                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
