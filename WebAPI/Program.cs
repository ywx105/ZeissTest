using DataDomain;
using DataDomain.Repository;
using Microsoft.EntityFrameworkCore;
using Services;
using WebAPI.WebSocketJobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHostedService, WebsocketService>();

builder.Services.AddDbContext<ZeissDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("ZeissDb")));

builder.Services.AddScoped<IMachinesPayloadService, MachinesPayloadService>();
builder.Services.AddScoped<IMachinesReponseRepository, MachinesReponseRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
