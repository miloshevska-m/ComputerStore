using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ComputerStore.data;
using ComputerStore.Core.Repositories;
using ComputerStore.data.Interfaces;
using ComputerStore.data.Repositories;
using ComputerStore.services.Interfaces;
using ComputerStore.services.Services;
using ComputerStore.services.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ComputerStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICategoryRepository, Categoryrepository>();
builder.Services.AddScoped<IProductRepository, Productrepository>();
builder.Services.AddScoped<Icategoryservice, Categoryservice>();
builder.Services.AddScoped<Iproductservice, Productservice>();
builder.Services.AddScoped<Idiscountservice, Discountservice>();

builder.Services.AddAutoMapper(typeof(mappingprofile));

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

app.Run();
