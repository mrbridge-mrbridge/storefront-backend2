using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activation_code = table.Column<string>(type: "character varying", nullable: true),
                    token = table.Column<string>(type: "character varying", nullable: true),
                    user_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_82fae97f905930df5d62a702fc9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    first_name = table.Column<string>(type: "character varying", nullable: true),
                    last_name = table.Column<string>(type: "character varying", nullable: true),
                    email = table.Column<string>(type: "character varying", nullable: true),
                    phone_number = table.Column<string>(type: "character varying", nullable: true),
                    password = table.Column<string>(type: "character varying", nullable: true),
                    oauth_id = table.Column<string>(type: "character varying", nullable: true),
                    role = table.Column<string>(type: "character varying", nullable: false),
                    activated = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    orderId = table.Column<Guid>(type: "uuid", nullable: true),
                    business_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cace4a159ff9f2512dd42373760", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOrdered = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderState = table.Column<string>(type: "text", nullable: false),
                    ShippingDetailsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_user_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreName = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    CurrencySymbol = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Stores_user_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    ChargeId = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.ChargeId);
                    table.ForeignKey(
                        name: "FK_Charges_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardDetails",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: true),
                    StripeId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardDetails", x => new { x.StoreId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_CreditCardDetails_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCardDetails_user_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discounts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    PageId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    PageCategory = table.Column<string>(type: "text", nullable: true),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    MainHeaderTextSize = table.Column<string>(type: "text", nullable: true),
                    SubHeaderTextsize = table.Column<string>(type: "text", nullable: true),
                    HeroImage = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Heading = table.Column<string>(type: "text", nullable: true),
                    SubHeading = table.Column<string>(type: "text", nullable: true),
                    MainColor = table.Column<string>(type: "text", nullable: true),
                    SubColor = table.Column<string>(type: "text", nullable: true),
                    FooterText = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    InstagramLink = table.Column<string>(type: "text", nullable: true),
                    FacebookLink = table.Column<string>(type: "text", nullable: true),
                    TwitterLink = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Publish = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_Pages_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipingDetails",
                columns: table => new
                {
                    ShippingDetailsId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipingDetails", x => x.ShippingDetailsId);
                    table.ForeignKey(
                        name: "FK_ShipingDetails_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipingDetails_user_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    ProductDescription = table.Column<string>(type: "text", nullable: true),
                    ProductCategory = table.Column<string>(type: "text", nullable: true),
                    UnitOfMeasurement = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Publish = table.Column<bool>(type: "boolean", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    PageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Photos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityPurchased = table.Column<int>(type: "integer", nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Purchases_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DateCommented = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => new { x.ProductId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_user_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewReplies",
                columns: table => new
                {
                    MerchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reply = table.Column<string>(type: "text", nullable: true),
                    DateReplied = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewReplies", x => x.MerchantId);
                    table.ForeignKey(
                        name: "FK_ReviewReplies_Reviews_CustomerId_ProductId",
                        columns: x => new { x.CustomerId, x.ProductId },
                        principalTable: "Reviews",
                        principalColumns: new[] { "ProductId", "CustomerId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewReplies_user_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charges_OrderId",
                table: "Charges",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardDetails_CustomerId",
                table: "CreditCardDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_StoreId",
                table: "Discounts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_StoreId",
                table: "Pages",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PageId",
                table: "Photos",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountId",
                table: "Products",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReplies_CustomerId_ProductId",
                table: "ReviewReplies",
                columns: new[] { "CustomerId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipingDetails_CustomerId",
                table: "ShipingDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipingDetails_StoreId",
                table: "ShipingDetails",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_MerchantId",
                table: "Stores",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "UQ_e12875dfb3b1d92d7d7c5377e22",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charges");

            migrationBuilder.DropTable(
                name: "CreditCardDetails");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "ReviewReplies");

            migrationBuilder.DropTable(
                name: "ShipingDetails");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
