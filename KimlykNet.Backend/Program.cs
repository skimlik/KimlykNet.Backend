using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

var originPolicyName = "_confidentialClientsOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);
builder.Configuration.AddEnvironmentVariables("DOCKER:");

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

var corsPolicy = (CorsPolicyBuilder corsBuilder) => 
    corsBuilder
        .WithOrigins("https://kimlyk.net")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: originPolicyName, policy => corsPolicy(policy));
    opt.AddDefaultPolicy(policy => corsPolicy(policy));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(originPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/ping", async context => await context.Response.WriteAsync("Pong"));
app.MapHealthChecks("/healthcheck");

app.Run();
