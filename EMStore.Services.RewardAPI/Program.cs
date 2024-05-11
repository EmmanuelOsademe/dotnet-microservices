using Microsoft.EntityFrameworkCore;
using EMStore.Services.RewardAPI.Data;
using EMStores.MessageBus;
using EMStore.Services.RewardAPI.Services.IServices;
using EMStore.Services.RewardAPI.Services;
using EMStore.Services.RewardAPI.Repository.IRepository;
using EMStore.Services.RewardAPI.Repository;
using EMStore.Services.RewardAPI.Extensions;
using EMStore.Services.RewardAPI.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new RewardService(new RewardRepository(optionBuilder.Options)));


builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IRewardRepository, RewardRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.UseAzureServiceBusConsumer();

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
