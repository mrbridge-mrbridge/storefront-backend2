using Application.Core;
using Application.Product;
using MediatR;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.Interfaces;
using Infrastructure.Photos;
using Stripe;
using Infrastructure.Stripe;

namespace Api.Extensions
{
	public static class AppSpecificExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config, string MyAllowSpecificOrigins)
        {
            services.AddMediatR(typeof(Create.Handler));
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.AddScoped<TokenService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ChargeService>();
			services.AddScoped<CouponService>();

			StripeConfiguration.ApiKey = config.GetValue<string>("StripeOptions:SecretKey");

            services.AddScoped<IStripeService, StripeService>();

			services.AddCors(options =>
			{
				options.AddPolicy(
					name: MyAllowSpecificOrigins,
					policy =>
					{
						policy.WithOrigins
						(
							"https://storefrontsmes.amalitech-dev.net",
							"http://localhost:3002",
							"http://localhost:5173"
						)
						.AllowAnyHeader()
						.AllowAnyMethod();
					}
				);
			});

			return services;
        }
    }
}
