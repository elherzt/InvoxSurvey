using DataLayer;
using DataLayer.CommonRepository;
using DataLayer.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Shared.Security;

var builder = WebApplication.CreateBuilder(args);

//DB Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
           .UseSnakeCaseNamingConvention()); // This method is part of the Npgsql.EntityFrameworkCore.PostgreSQL.NamingConventions namespace




// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hasher service
builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

//JWT Configuration
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();

AuthConfig.ConfigureJwt(builder.Services, builder.Configuration);



// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

// Migrations and seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // apply pending migrations

    var hasher = services.GetRequiredService<IPasswordHasherService>();
    await DbSeeder.SeedDataAsync(dbContext, hasher); //seed initial data
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
