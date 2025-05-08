using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.Infrastructure.Persistence;
using ConsumerComplaints.Infrastructure.Repositories;
using CustomerComplaints.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ← TUTAJ dodajesz swoje repozytoria:
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// Dodanie bazy danych SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=C:/data/consumer_complaints.db"));


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