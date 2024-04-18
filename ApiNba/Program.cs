using Microsoft.EntityFrameworkCore;
using ApiNba.Data;
using ApiNba.Repositories;
using Microsoft.OpenApi.Models;
using ApiNba.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();
builder.Services.AddTransient<HelperTools>();

builder.Services.AddTransient<RepositoryEntradas>();
builder.Services.AddTransient<RepositoryJugadores>();
builder.Services.AddTransient<RepositoryNba>();
builder.Services.AddTransient<RepositoryUsuarios>();

string connectionString = builder.Configuration.GetConnectionString("SqlAzure");

builder.Services.AddControllers();

builder.Services.AddDbContext<NbaContext>(options => options.UseSqlServer(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api NBA Valentin",
        Description = "Api de cositas de la NBA",
        Version = "v1"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api Nba Valentin");
    options.RoutePrefix = "";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
