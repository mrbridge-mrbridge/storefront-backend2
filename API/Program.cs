using Api.Extensions;
using Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("aspbackend"))
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddServices(builder.Configuration, MyAllowSpecificOrigins);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins(
                    "https://storefrontsmes.amalitech-dev.net",
                    "http://localhost:3002",
                    "http://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment()) { }
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

// using var scope = app.Services.CreateScope();
// var services = scope.ServiceProvider;
// try
// {
//     var context = services.GetRequiredService<AppDataContext>();
//     await context.Database.MigrateAsync(); // equivalent database Update
//     await SeedData.Seed(context);
// }
// catch (Exception ex)
// {
//     var logger = services.GetRequiredService<ILogger<Program>>();
//     logger.LogError(ex, "An Error occured during migration");
// }

app.Run();
