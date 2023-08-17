using GalaxyBoardsAPI.Data;
using Microsoft.EntityFrameworkCore;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Search;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*");
        policy.WithHeaders("*");
        policy.WithMethods("*");
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<GalaxyBoardsContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseNpgsql(configuration.GetConnectionString("GalaxyBoardsDbPostgres"));
});
builder.Services.AddScoped<IEntityRepository<Project>, EntityRepository<Project>>();
builder.Services.AddScoped<IEntityRepository<User>, EntityRepository<User>>();
builder.Services.AddScoped<IEntityRepository<Ticket>, EntityRepository<Ticket>>();
builder.Services.AddScoped<IEntitySearch<Ticket>, EntitySearch<Ticket>>();
builder.Services.AddScoped<IEntityRepository<TicketPlacement>, EntityRepository<TicketPlacement>>();
builder.Services.AddScoped<IEntityRepository<Board>, EntityRepository<Board>>();
builder.Services.AddScoped<IEntityRepository<BoardColumn>, EntityRepository<BoardColumn>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GalaxyBoards API",
        Description = "An ASP.NET Core Web API for creating and managing Kanban-style boards"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

using (var scope = app.Services.CreateScope())
{
    using (var dbContext = scope.ServiceProvider.GetRequiredService<GalaxyBoardsContext>())
    {
        dbContext.Database.EnsureCreated();
        DatabaseInitializer.Seed(dbContext);
    }
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
