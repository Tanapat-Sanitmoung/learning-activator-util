using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessagePrinter(this IServiceCollection services)
    {
        services.TryAddTransient<IMessagePrinter, MessagePrinter>();
        services.TryAddTransient<IMessageBag, DefaultMessageBag>();
        return services;
    }

    public static IServiceCollection AddMessagePrinter<TInstance>(this IServiceCollection services, Action<MessagePrinterConfiguration> config)
         where TInstance : class
    {
        return services.AddMessagePrinter<TInstance, TInstance>(config);
    }

    public static IServiceCollection AddMessagePrinter<IServiceType, IImplementType>(
            this IServiceCollection services, 
            Action<MessagePrinterConfiguration> config
        )
        where IServiceType: class
        where IImplementType : class, IServiceType
    {
        services.AddMessagePrinter();

        var cfg = new MessagePrinterConfiguration();
        config.Invoke(cfg);

        var bagType = cfg.MessageBagType;
        var serviceType = typeof(IServiceType);
        var serviceImplementType = typeof(IImplementType);

        if (bagType is null)
        {
            services.TryAdd(
                new ServiceDescriptor(
                    serviceType: serviceType,
                    implementationType: serviceImplementType,
                    lifetime: ServiceLifetime.Scoped));

            return services;
        }
        else
        {
            services.TryAdd(new ServiceDescriptor(
                serviceType: bagType,
                implementationType: bagType,
                lifetime: ServiceLifetime.Transient
            ));

            var serviceDescriptor = new ServiceDescriptor(
                    serviceType: serviceType,
                    factory: (sp) =>
                    {
                        var bag = sp.GetRequiredService(bagType);
                        var printer = ActivatorUtilities.CreateInstance(sp, typeof(MessagePrinter), [bag]);
                        return ActivatorUtilities.CreateInstance(sp, serviceImplementType, [printer]);
                    },
                    lifetime: ServiceLifetime.Transient
                );

            services.TryAdd(serviceDescriptor);
        }


        return services;
    }
}
