using EMStore.Services.ShoppingCartAPI.Data;
using EMStore.Services.ShoppingCartAPI.Extensions;
using EMStore.Services.ShoppingCartAPI.Repositories;
using EMStore.Services.ShoppingCartAPI.Repositories.Interfaces;
using EMStore.Services.ShoppingCartAPI.Services;
using EMStore.Services.ShoppingCartAPI.Services.IServices;
using EMStore.Services.ShoppingCartAPI.Utility;
using EMStores.MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendAPIAuthenticationHttpClientHandler>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(
    builder.Configuration["ServiceUrls:ProductAPI"] ?? string.Empty
    )).AddHttpMessageHandler<BackendAPIAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(
    builder.Configuration["ServiceUrls:CouponAPI"] ?? string.Empty
    )).AddHttpMessageHandler<BackendAPIAuthenticationHttpClientHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as follows: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});

builder.AddAppAuthentication();

builder.Services.AddAuthorization();
builder.Services.AddScoped<ICartRepository, CartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
