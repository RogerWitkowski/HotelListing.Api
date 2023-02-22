using HotelListing.API.Configurations;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//!connectionString
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//! Identity Core
builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HotelListingDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//!Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
        policyBuilder.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod()
    );
});
//!Serilog
builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));
//!AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
//!GenericRepository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped(typeof(IHotelRepository), typeof(HotelRepository));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//!Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
//!Cors
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();