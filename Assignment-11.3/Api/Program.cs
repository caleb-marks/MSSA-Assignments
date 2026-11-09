// Caleb Marks

using Caleb_Marks_Assignment_11._3_Api.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register controllers, Swagger, & SQLite context
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GameContext>(options => options.UseSqlite("Data Source=games.db"));

WebApplication app = builder.Build();

// Show Swagger test page; development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Build database file if missing
using (IServiceScope scope = app.Services.CreateScope())
{
    GameContext context = scope.ServiceProvider.GetRequiredService<GameContext>();
    context.Database.EnsureCreated();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
