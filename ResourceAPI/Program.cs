
//Step 02. To generate the C# models:
//Scaffold-DbContext "Server=.\sqlexpress;Database=Resources;Trusted_Connection=true;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

//Step 03. Add the using statement
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Step 11. Add Cors functionality to determine what websites can access the data in this application
//CORS stands for Cross Origin Resource Sharing and by default browsers use this to block websites from requesting data unless that website has permission to do so. This code below determines what websites have access to CORS with this API.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("OriginPolicy", "http://localhost:3000");
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Step 04. Add the ResourcesContext Service
//This will initialize the database connection to be used in the controllers
builder.Services.AddDbContext<ResourceAPI.Models.ResourcesContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ResourcesDB"));//The string should match the connectionString name in appsettings.json
    }
    );

//Step 05. Scaffold a new API controller using Entity Framework - Scaffold Categories choosing the Categories model, the ResourcesContext for DataContext. After walk thru the code in the controller and test using the browser (Swagger).
//Looking for next step? Open ResourcesController.

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
