using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Configuration.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var identityConnectionString = builder.Configuration.GetConnectionString("IdentityDb");
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlServer(identityConnectionString);
});

builder.Services
    .AddIdentityCore<ApplicationUser>(
        opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.User.RequireUniqueEmail = true;
        })
    .AddRoles<ApplicationRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddTransient<ITokenBuilder, TokenBuilder>();

builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.SectionName));

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddHostedService<IdentityInitializer>(services => new IdentityInitializer(services));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();
app.Map("/ping", async context => await context.Response.WriteAsync("Pong"));

app.Run();
