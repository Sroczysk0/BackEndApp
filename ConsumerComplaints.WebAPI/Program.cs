using System.Security.Claims;
using System.Text;
using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.Infrastructure.Persistence;
using ConsumerComplaints.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ConsumerComplaints.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// === 1. Kontrolery, Swagger ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ConsumerComplaints.WebAPI",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Wpisz: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer", // <--- ma być lowercase
        BearerFormat = "JWT"
    });



    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.CommandTimeout(10)
    ));


// === 3. Tożsamość i JWT ===
builder.Services.AddIdentity<UserEntity, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });



builder.Services.AddAuthorization();

// === 4. Rejestracja repozytoriów ===
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();

// === 5. Budowanie aplikacji ===
var app = builder.Build();

// === 6. Middleware ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // MUSI być przed Authorization
app.UseAuthorization();
app.MapControllers();    // MUSI być!
app.Run();
