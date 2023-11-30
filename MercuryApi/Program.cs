global using MercuryApi.Data.Entities;
using MercuryApi.BLL;
using MercuryApi.Data;
using MercuryApi.Data.Repository;
using MercuryApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAuthentication().AddCookie("default");
builder.Services.AddDbContext<MercuryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MercuryDb")));
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<ITicketBusinessLogic, TicketBusinessLogic>();
builder.Services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
builder.Services.AddScoped<ITeamBusinessLogic, TeamBusinessLogic>();
builder.Services.AddScoped<IProjectBusinessLogic, ProjectBusinessLogic>();
builder.Services.AddScoped<IStatusBusinessLogic, StatusBusinessLogic>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string signingKey = builder.Configuration.GetSection("SigningKey").Value ?? throw new Exception("Coud not find signing key.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(signingKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
