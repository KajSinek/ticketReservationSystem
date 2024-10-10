using GuardNet;

namespace VismaIdella.Vips.Abstractions.Extensions;

public static class ServiceCollectionExtensions
{
    public static T ConfigureOptions<T>(this WebApplicationBuilder builder, string sectionName)
        where T : class, new()
    {
        Guard.NotNull(builder, nameof(builder));

        var options = new T();
        IConfigurationSection section = builder.Configuration.GetSection(sectionName);
        if (section.Exists())
        {
            section.Bind(options);
        }

        builder.Services.Configure<T>(section).AddSingleton(options);

        return options;
    }
}
