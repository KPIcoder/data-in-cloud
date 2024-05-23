using Microsoft.EntityFrameworkCore;
using DataInCloud.Dal.Meal;
using DataInCloud.Model.Meal;
using DataInCloud.Orchestrators;
using DataInCloud.Model.Restaurant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(sp => new MongoDbContext(
    builder.Configuration.GetConnectionString("MongoDbConnection"),
    builder.Configuration["MongoDbDatabaseName"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IMealOrchestrator, MealOrchestrator>();

builder.Services.AddScoped<IMealRepository, MealRepository>();

builder.Services.AddScoped<IRestaurantOrchestrator, RestaurantOrchestrator>();

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();

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

// Make Program accessible
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }
