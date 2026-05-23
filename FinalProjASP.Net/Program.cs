using System.Security.Claims;
using System.Text;
using CryptoProj.API.Middlewares;
using FinalProjASP.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Storage;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddEndpointsApiExplorer();



builder.Services.AddTransient<GlobalExceptionHandler>();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<DataContext>(opt=>opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BestDataBaseASPNetProj;Trusted_Connection=True;"));
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<DynamoDBContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "role",
            NameClaimType = ClaimTypes.NameIdentifier,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]!))
        };
    });

builder.Services.AddAuthorization(opt => opt
    .AddPolicy("Admin", 
        policy => policy
            .RequireRole("Admin")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddFinalProjServices();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();