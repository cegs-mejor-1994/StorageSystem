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

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IInputInventoriesRepository, InputInventoriesRepository>();
builder.Services.AddScoped<IMeasurementConversionsRepository, MeasurementConversionsRepository>();
builder.Services.AddScoped<IMeasurementUnitsRepository, MeasurementUnitsRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductsDetailsRepository, ProductsDetailsRepository>();
builder.Services.AddScoped<IProductionGapsRepository, ProductionGapsRepository>();
builder.Services.AddScoped<IRawMaterialsRepository, RawMaterialsRepository>();
builder.Services.AddScoped<IRecipesRepository, RecipesRepository>();
builder.Services.AddScoped<IRecipeDetailsRepository, RecipeDetailsRepository>();
builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();

builder.Services.AddScoped<ICategoriesUnitOfWork, CategoriesUnitOfWork>();
builder.Services.AddScoped<IClientsUnitOfWork, ClientsUnitOfWork>();
builder.Services.AddScoped<IInputInventoriesUnitOfWork, InputInventoriesUnitOfWork>();
builder.Services.AddScoped<IMeasurementConversionsUnitOfWork, MeasurementConversionsUnitOfWork>();
builder.Services.AddScoped<IMeasurementUnitsUnitOfWork, MeasurementUnitsUnitOfWork>();
builder.Services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();
builder.Services.AddScoped<IProductsDetailsUnitOfWork, ProductsDetailsUnitOfWork>();
builder.Services.AddScoped<IProductionGapsUnitOfWork, ProductionGapsUnitOfWork>();
builder.Services.AddScoped<IRawMaterialsUnitOfWork, RawMaterialsUnitOfWork>();
builder.Services.AddScoped<IRecipeDetailsUnitOfWork, RecipeDetailsUnitOfWork>();
builder.Services.AddScoped<IRecipesUnitOfWork, RecipesUnitOfWork>();
builder.Services.AddScoped<IReferencesUnitOfWork, ReferencesUnitOfWork>();
builder.Services.AddScoped<ISuppliersUnitOfWork, SuppliersUnitOfWork>();

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
