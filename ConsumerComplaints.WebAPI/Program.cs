using ConsumerComplaints.Infrastructure.Persistence;
using ConsumerComplaints.Infrastructure.Repositories;
using CustomerComplaints.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodanie bazy danych SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytorium
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();

var app = builder.Build();

// Konfiguracja API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();