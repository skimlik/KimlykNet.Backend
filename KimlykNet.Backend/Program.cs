using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Swagger;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserContextAccessor, UserContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithConfiguration();

builder.Services.AddAspNetIdentity(builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});

builder.Services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

builder.Services.AddHostedService(services => new IdentityInitializer(services));

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
app.MapGet("/ping", async context => await context.Response.WriteAsync("Pong"));

app.Run();
