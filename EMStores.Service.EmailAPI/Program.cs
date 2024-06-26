using EMStore.Services.EmailAPI.Data;
using EMStore.Services.EmailAPI.Extensions;
using EMStore.Services.EmailAPI.Messaging;
using EMStore.Services.EmailAPI.Repositories;
using EMStore.Services.EmailAPI.Repositories.Interfaces;
using EMStore.Services.EmailAPI.Services;
using EMStore.Services.EmailAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
var smtpSetting = new SMTPSetting();
builder.Configuration.GetSection("SMTP").Bind(smtpSetting);

optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//builder.Services.AddSingleton(new EmailService(optionBuilder.Options));
builder.Services.AddSingleton(new EmailService(new EmailRepository(optionBuilder.Options), smtpSetting));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddControllers();

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
