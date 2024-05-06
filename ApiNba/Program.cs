using Microsoft.EntityFrameworkCore;
using ApiNba.Data;
using ApiNba.Repositories;
using Microsoft.OpenApi.Models;
using ApiNba.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors.Security;
using NSwag;
using Microsoft.Extensions.Azure;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

//DEBEMOS PODER RECUPERAR UN OBJETO INYECTADO EN CLASES
//QUE NO TIENEN CONSTRUCTOR 
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret issuerSecret = await secretClient.GetSecretAsync("SecretIssuer");
KeyVaultSecret audienceSecret = await secretClient.GetSecretAsync("SecretAudience");
KeyVaultSecret secretKeySecret = await secretClient.GetSecretAsync("SecretKey");
KeyVaultSecret secret = await secretClient.GetSecretAsync("SecretNba"); //Aqui ponemos el nombre del secret

string connectionString = secret.Value;
string issuer = issuerSecret.Value;
string audience = audienceSecret.Value;
string secretKey = secretKeySecret.Value;

builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();

HelperActionServicesOAuth helper = new HelperActionServicesOAuth(issuer,audience,secretKey);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

builder.Services.AddTransient<HelperTools>();

builder.Services.AddTransient<RepositoryEntradas>();
builder.Services.AddTransient<RepositoryJugadores>();
builder.Services.AddTransient<RepositoryNba>();
builder.Services.AddTransient<RepositoryUsuarios>();

builder.Services.AddControllers();

builder.Services.AddDbContext<NbaContext>(options => options.UseSqlServer(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api OAuth NBA";
    document.Description = "Api con seguridad 2024";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();
//app.UseSwagger();
app.UseOpenApi();
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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
