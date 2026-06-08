using Microsoft.EntityFrameworkCore;
using RH_Azure.Context;
using RH_Azure.Repository;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ───────────────────────────────────────────────────────────────

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "RH Azure API", Version = "v1" });
});

// SQL Database via EF Core
builder.Services.AddDbContext<RHContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Repository com Table Storage connection string
builder.Services.AddScoped<FuncionarioRepository>(provider =>
{
    var context = provider.GetRequiredService<RHContext>();
    var tableConnStr = builder.Configuration.GetConnectionString("TableStorageConnection");
    return new FuncionarioRepository(context, tableConnStr);
});

// ─── App ────────────────────────────────────────────────────────────────────

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
