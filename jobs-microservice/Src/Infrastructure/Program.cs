using Application.Core;
using DotNetEnv;
using Hangfire;
using Jobs.Extensions;



var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.ConfigureServices();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.ConfigureHangFire(builder.Configuration);
builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddSwagger();
builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSwagger();
app.UseHangfireDashboard("/hangfire");

app.MapControllers().RequireAuthorization();

app.MapGet("api/jobs/health", () => Results.Ok("ok"));
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
