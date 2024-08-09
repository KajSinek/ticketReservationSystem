using BackgroundJobs.Api;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// app insights
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);

builder.AddCustomHangfire();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Enable Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Hangfire Dashboard endpoint
app.MapHangfireDashboard();

app.Run();
