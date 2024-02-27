using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProyectoFinal.Entities;
using ProyectoFinal.Utils;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// for the ORM configuration
services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
services.AddDatabaseDeveloperPageExceptionFilter();

// For identity Configuration
services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = false;
});


// JWT configuration
var JWTConfig = configuration.GetSection("JWT");
var issuer = JWTConfig.GetSection("Issuer").Get<string>();
var audience = JWTConfig.GetSection("Audience").Get<string>();
var keys = JWTConfig.GetSection("Key").Get<string>();

services.AddHttpContextAccessor();
services.AddAuthorization();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keys!))
    };
});


// build the app
var app = builder.Build();

// seed config
await new SeedConfig(app).SeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrationsEndPoint();
}

app.UseAuthorization();
app.UseAuthentication();
app.UseHttpsRedirection();

app.Run();