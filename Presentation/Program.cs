using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using AgroConn.Infrastructure.Context;
using Application.RepositoryInterface;
using Application.ServicesImplementation;
using Application.ServicesInterface;
using Infrastructure.RepositoryImplementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers (enums are sent/received as text, e.g. "Business") ----------
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

// ---------- Database (MySQL) ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!));

// ---------- Repositories ----------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IFarmerProfileRepository, FarmerProfileRepository>();
builder.Services.AddScoped<IBuyerProfileRepository, BuyerProfileRepository>();
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<IProduceListingRepository, ProduceListingRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ---------- Services ----------
builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<IFarmerProfileServices, FarmerProfileServices>();
builder.Services.AddScoped<IBuyerProfileServices, BuyerProfileServices>();
builder.Services.AddScoped<ICropServices, CropServices>();
builder.Services.AddScoped<IProduceListingServices, ProduceListingServices>();
builder.Services.AddScoped<IReservationServices, ReservationServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

// ---------- JWT ----------
var jwt = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // keep the claim names exactly as TokenServices wrote them
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // was missing - without it the token is never read
app.UseAuthorization();

app.MapControllers();

app.Run();
