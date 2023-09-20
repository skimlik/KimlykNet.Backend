using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Swagger;
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
builder.Services.AddCustomAuthorization();

builder.Services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

builder.Services.AddHostedService(services => new IdentityInitializer(services));
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/ping", async context => await context.Response.WriteAsync("Pong"));
app.MapHealthChecks("/healthcheck");

app.Run();
