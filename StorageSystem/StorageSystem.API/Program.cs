using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Implementations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(op => op.UseSqlServer("name=CadenaStorage"));

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IMeasurementUnitsRepository, MeasurementUnitsRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();
builder.Services.AddScoped<IRawMaterialsRepository, RawMaterialsRepository>();
builder.Services.AddScoped<IInputInventoriesRepository, InputInventoriesRepository>();

builder.Services.AddScoped<IMeasurementUnitsUnitOfWork, MeasurementUnitsUnitOfWork>();
builder.Services.AddScoped<ICategoriesUnitOfWork, CategoriesUnitOfWork>();
builder.Services.AddScoped<ISuppliersUnitOfWork, SuppliersUnitOfWork>();
builder.Services.AddScoped<IRawMaterialsUnitOfWork, RawMaterialsUnitOfWork>();
builder.Services.AddScoped<IInputInventoriesUnitOfWork, InputInventoriesUnitOfWork>();

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

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.Run();
