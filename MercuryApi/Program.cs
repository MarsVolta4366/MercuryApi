global using MercuryApi.Data.Entities;
using MercuryApi.Data;
using MercuryApi.Data.Repository;
using MercuryApi.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAuthentication().AddCookie("default");
builder.Services.AddDbContext<MercuryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MercuryDb")));
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

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

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials());

app.UseCookiePolicy(
    new CookiePolicyOptions()
    {
        Secure = CookieSecurePolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.None,
        //HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None
    });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
