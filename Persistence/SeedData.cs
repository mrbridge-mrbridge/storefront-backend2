using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class SeedData
    {
        public static async Task Seed(AppDataContext context)
        {
            var customers = new List<Customer>
                {
                    new Customer
                    {
                        Email = "chipo@test.com",
                        FirstName = "Chipo",
                        LastName = "Poolo",
                        PhoneNumber = "0298827",
                        Password = "Pa$$w0rd",
                    },
                     new Customer
                    {
                        Email = "boole@test.com",
                        FirstName = "Boole",
                        LastName = "ata",
                        PhoneNumber = "0298827",
                        Password = "Pa$$w0rd",
                    },
                      new Customer
                    {
                        Email = "giri@test.com",
                        FirstName = "Gire",
                        LastName = "Gire",
                        PhoneNumber = "0298827",
                        Password = "Pa$$w0rd",
                    },
                       new Customer
                    {
                        Email = "riginal@test.com",
                        FirstName = "Oroginal",
                        LastName = "Fawat",
                        PhoneNumber = "0298827",
                        Password = "Pa$$w0rd",
                    },

                };
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            var merchants = new List<Merchant>
                {
                    new Merchant
                    {
                        Email = "hansome@mer.com",
                        FirstName = "hansome",
                        LastName = "General",
                        PhoneNumber = "02036478",
                        Password = "Pa$$w0rd",

                    },
                    new Merchant
                    {
                        Email = "maili@mer.com",
                        FirstName = "linsu",
                        LastName = "maili",
                        PhoneNumber = "02036478",
                        Password = "Pa$$w0rd",

                    },
                    new Merchant
                    {
                        Email = "grendel@mer.com",
                        FirstName = "Grendel",
                        LastName = "Wiglaf",
                        PhoneNumber = "020037478",
                        Password = "Pa$$w0rd",

                    },
                    new Merchant
                    {
                        Email = "smith@mer.com",
                        FirstName = "Sonia",
                        LastName = "Smith",
                        PhoneNumber = "0206737478",
                        Password = "Pa$$w0rd",

                    },
                    new Merchant
                    {
                        Email = "Kofi@mer.com",
                        FirstName = "Kofi",
                        LastName = "yeboa",
                        PhoneNumber = "02036478",
                        Password = "Pa$$w0rd",

                    }
                };
            await context.Merchants.AddRangeAsync(merchants);
            await context.SaveChangesAsync();

            var mer = await context.Merchants.ToListAsync();

            var stores = new List<Store>
            {
                new Store
                {
                    MerchantId = mer[1].Id,
                },
                new Store
                {
                    MerchantId = mer[1].Id,
                },
                new Store
                {
                    MerchantId = mer[4].Id,
                },
                new Store
                {
                    MerchantId = mer[4].Id,
                },
                new Store
                {
                    MerchantId = mer[0].Id,
                },
                new Store
                {
                    MerchantId = mer[2].Id,
                },

            };
            await context.Stores.AddRangeAsync(stores);
            await context.SaveChangesAsync();

            var sto = await context.Stores.ToListAsync();

            var products = new List<Product>
            {
                new Product
                {
                    ProductCategory = "Finace",
                    ProductDescription = "Banking and other services",
                    ProductName = "Life assurance",
                    StoreId = sto[0].StoreId,
                },
                new Product
                {
                    ProductCategory = "Inventory",
                    ProductDescription = "cement",
                    ProductName = "Gacem",
                    StoreId = sto[3].StoreId,
                },
                new Product
                {
                    ProductCategory = "Inventory",
                    ProductDescription = "Some Description",
                    ProductName = "Iron Rod",
                    StoreId = sto[1].StoreId,
                },
                new Product
                {
                    ProductCategory = "someCate",
                    ProductDescription = "SomecategoryDescription",
                    ProductName = "SomeProdut",
                    StoreId = sto[0].StoreId,
                },
                
                new Product
                {
                    ProductCategory = "sggs",
                    ProductDescription = "hkahua",
                    ProductName = "jhsghka",
                    StoreId = sto[4].StoreId,
                },

            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // // var cus = await context.Customers.ToListAsync();
            // var pro = await context.Products.ToListAsync();

            // var purchases = new List<Purchase>
            // {
            //     new Purchase
            //     { 
            //         CustomerId = cus[3].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(9),
            //         ProductId = pro[1].ProductId,
            //         QuantityPurchased = 30,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[2].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(5),
            //         ProductId = pro[4].ProductId,                   
            //         QuantityPurchased = 5,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[3].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(1),
            //         ProductId = pro[3].ProductId,
            //         QuantityPurchased = 10,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[2].Id,
            //         DatePurchased = DateTime.UtcNow,
            //         ProductId = pro[0].ProductId,
            //         QuantityPurchased = 15,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[0].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(2),
            //         ProductId = pro[4].ProductId,
            //         QuantityPurchased = 30,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[3].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(2),
            //         ProductId = pro[1].ProductId,
            //         QuantityPurchased = 30,
            //     },
            //     new Purchase
            //     {
            //         CustomerId = cus[0].Id,
            //         DatePurchased = DateTime.UtcNow.AddDays(0),
            //         ProductId = pro[1].ProductId,
            //         QuantityPurchased = 30,
            //     },


            // };
            // await context.Purchases.AddRangeAsync(purchases);
            // await context.SaveChangesAsync();
            
        }
    }
}