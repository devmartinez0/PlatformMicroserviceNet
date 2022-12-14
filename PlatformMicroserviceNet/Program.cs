using Microsoft.EntityFrameworkCore;
using PlatformMicroserviceNet.Data;
using PlatformMicroserviceNet.SyncDataServices.Http;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registering for DI
builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

Debug.WriteLine($"--> COMMAND ENDPOINT {builder.Configuration["CommandService"]}");

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

PrepDb.PrepPopulation(app);

app.Run();
