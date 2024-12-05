using BackgroundJobs.Api;
using Hangfire;

const string AppName = "Background Jobs API";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:5000", "http://127.0.0.1:5000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

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

// Use CORS in your app
app.UseCors("AllowLocalhost");

app.MapControllers();

// hangfire
/*app.MapHangfireDashboardWithAuthorizationPolicy(
    "hangfire",
    "/hangfire",
    new DashboardOptions
    {
        Authorization = [],
        DarkModeEnabled = true,
        DashboardTitle = AppName
    }
);*/

app.MapHangfireDashboard(
    "/hangfire",
    new DashboardOptions { DashboardTitle = AppName, Authorization = [] }
);

app.Run();
