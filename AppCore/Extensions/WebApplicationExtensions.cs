namespace AppCore.Extensions;

public static class WebApplicationExtensions
{
    public static void AddCustomApplicationComponents(this WebApplicationBuilder builder)
    {
        builder.AddMediatorHandlers();
    }

    public static void AddMediatorHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(WebApplicationExtensions)));
    }
}
