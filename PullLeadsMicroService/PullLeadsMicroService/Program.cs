using Microsoft.EntityFrameworkCore;
using PullLeadsMicroService.BackgroundTasks;
using PullLeadsMicroService.Data;
using PullLeadsMicroService.Repositories;
using PullLeadsMicroService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddFile("Logs/pull-microservice-log-{Date}.txt"); // Add file-based logging
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

// Add Pull Service
builder.Services.AddScoped<IPullService, PullService>();

// Add Background Service for Timer-Based Execution
builder.Services.AddHostedService<PullBackgroundService>();

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
