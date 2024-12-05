using Helpers.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TRS.CoreApi.Database;
using TRS.CoreApi.Extensions;
using BackgroundJobs.Api;
using Refit;
using TRS.CoreApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped(typeof(IBaseDbRequests), typeof(BaseDbRequests<AppDbContext>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add Hangfire services from the BackgroundJobs project
builder.Services.AddCustomServices();

builder.Services.AddRefitClient<IBackgroundJobCall>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://backgroundjobs:5000"));

builder.AddCustomApplicationComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
