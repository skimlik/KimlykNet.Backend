using System.Text.Json;
using System.Threading.Channels;

using KimlykNet.Backend.Background;
using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Configuration;
using KimlykNet.Backend.Infrastructure.Database;
using KimlykNet.Backend.Infrastructure.Swagger;
using KimlykNet.Contracts;
using KimlykNet.Data;
using KimlykNet.Data.Abstractions;
using KimlykNet.Services;
using KimlykNet.Services.Abstractions.Clients;
using KimlykNet.Services.Abstractions.Configuration;
using KimlykNet.Services.Abstractions.Services;
using KimlykNet.Services.Clients;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

using CorsMiddleware = KimlykNet.Backend.Infrastructure.Configuration.CorsMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);
builder.Configuration.AddEnvironmentVariables("DOCKER:");

builder.Services.AddCors(opt =>
{
    var config = builder.Configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>();
    opt.AddPolicy(name: config.PolicyName , policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(config.AllowedOrigins));
    opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(config.AllowedOrigins));
});

// Add services to the container.

builder.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserContextAccessor, UserContextAccessor>();
builder.Services.AddControllers();

builder.Services.Configure<NotificationsSettings>(builder.Configuration.GetSection(NotificationsSettings.SectionName));
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection(CorsSettings.SectionName));
builder.Services.Configure<EncoderOptions>(builder.Configuration.GetSection(EncoderOptions.SectionName));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithConfiguration();

builder.Services.AddAspNetIdentity(builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

builder.Services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

builder.Services.AddHostedService(services => new IdentityInitializer(services));
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityContext>()
    .AddDbContextCheck<DataContext>();

builder.Services.AddHttpClient<INotificationClient, NotificationClient>();
builder.Services.AddTransient<INotificator, NotificationService>();
builder.Services.AddSingleton<IIdEncoder, IdEncoder>();

var channelOptions = new BoundedChannelOptions(1000)
{
    SingleWriter = false,
    SingleReader = true,
    FullMode = BoundedChannelFullMode.Wait
};
builder.Services.AddSingleton(Channel.CreateBounded<ApplicationNotification>(channelOptions));
builder.Services.AddSingleton<NotificationChannel>();
builder.Services.AddHostedService<NotificationChannelConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/ping", async context => await context.Response.WriteAsync("Pong"));
app.MapGet("/.well-known/acme-challenge", () =>
{

});
app.MapHealthChecks("/healthcheck");
app.Run();
