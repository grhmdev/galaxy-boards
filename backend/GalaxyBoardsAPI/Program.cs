using GalaxyBoardsAPI.Data;
using Microsoft.EntityFrameworkCore;
using GalaxyBoardsAPI.Data.Repository;
using GalaxyBoardsAPI.Data.Entities;
using GalaxyBoardsAPI.Data.Search;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
