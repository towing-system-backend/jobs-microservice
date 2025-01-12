using Application.Core;
using DotNetEnv;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Job.Infrastructure;
using jobs_microservice.Utils.Core.Src.Application.JobService;
using jobs_microservice.Utils.Core.Src.Application.MesssageBrokerService;
using jobs_microservice.Utils.Core.Src.Infrastructure.HangFireService;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.AddSingleton<MongoEventStore>();
builder.Services.AddScoped<IEventStore, MongoEventStore>();
builder.Services.AddScoped<IdService<string>, GuidGenerator>();
builder.Services.AddScoped<IMessageBrokerService, RabbitMQService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<JobController>();
builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
});

var certSection = builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");
certSection["Path"] = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel_CertificatesDefault_Path")!;
certSection["Password"] = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel_CertificatesDefault_Password")!;

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Job API", Version = "v1" });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("API_GATEWAY_URL")!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
        };
    });

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMongoStorage(
        Environment.GetEnvironmentVariable("CONNECTION_URI"), 
        Environment.GetEnvironmentVariable("DATABASE_NAME"), 
        new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            Prefix = "hangfire.mongo",
            CheckConnection = true
        }
    )
);

builder.Services.AddHangfireServer();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<OrderStatusUpdatedConsumer>();
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")!), h =>
        {
            h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!);
            h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!);
        });

        configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHangfireDashboard();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job v1");
    c.RoutePrefix = string.Empty;
});
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("api/jobs/health", () => Results.Ok("ok"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
