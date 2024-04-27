using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMessagePrinter();
builder.Services.AddMessagePrinter<FooFighter>(() => typeof(FooMessageBag));
builder.Services.AddMessagePrinter<NonFooFighter>(() => typeof(NonFooMessageBag));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/default", ([FromServices] IMessagePrinter printer) =>
{
    return printer.Print("Magic");
})
.WithName("default")
.WithOpenApi();

app.MapGet("/foo", ([FromServices] FooFighter fooFighter) =>
{
    return fooFighter.DoMagic();
})
.WithName("foo")
.WithOpenApi();

app.MapGet("/non-foo", ([FromServices] NonFooFighter nonFooFighter) =>
{
    return nonFooFighter.DoSomethingElse();
})
.WithName("non-foo")
.WithOpenApi();

app.Run();

public class FooFighter
{
    private readonly IMessagePrinter _massagePrinter;

    public FooFighter(IMessagePrinter messagePrinter)
    {
        _massagePrinter = messagePrinter;
    }

    public string DoMagic()
    {
        return _massagePrinter.Print("Magic");
    }
}

public class NonFooFighter
{
    private readonly IMessagePrinter _messagePrinter;

    public NonFooFighter(IMessagePrinter messagePrinter)
    {
        _messagePrinter = messagePrinter;
    }

    public string DoSomethingElse()
    {
        return _messagePrinter.Print("SomethingElse");
    }
}

public interface IMessagePrinter
{
    string Print(string key);
}

public class MessagePrinter : IMessagePrinter
{
    private readonly IMessageBag _messageBag;

    public MessagePrinter(IMessageBag messageBag)
    {
        _messageBag = messageBag;
    }

    public string Print(string key)
    {
        var msg = _messageBag.Get(key);
        return msg;
    }
}

public interface IMessageBag
{
    string Get(string key);
}

public class FooFighterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FooFighterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public FooFighter Create()
    {
        var msgBag = _serviceProvider.GetRequiredService<FooMessageBag>();
        var msgPrinter = ActivatorUtilities.CreateInstance<MessagePrinter>(_serviceProvider, [msgBag]);
        return ActivatorUtilities.CreateInstance<FooFighter>(_serviceProvider, [msgPrinter]);
    }
}


public class DefaultMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"Default message for: {key}";
    }
}

public class FooMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"Foo message for: {key}";
    }
}

public class NonFooMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"NonFoo message for: {key}";
    }
}

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessagePrinter(this IServiceCollection services)
    {
        services.TryAddTransient<IMessagePrinter, MessagePrinter>();
        services.TryAddTransient<IMessageBag, DefaultMessageBag>();
        return services;
    }

    public static IServiceCollection AddMessagePrinter<TInstance>(this IServiceCollection services, Func<Type> opt)
         where TInstance : class
    {
        services.AddMessagePrinter();

        var bagType = opt.Invoke();

        var bagDescriptor = new ServiceDescriptor(
            serviceType: bagType,
            implementationType: bagType,
            lifetime: ServiceLifetime.Transient
        );

        services.TryAdd(bagDescriptor);

        var serviceType = typeof(TInstance);
        var serviceDescriptor = new ServiceDescriptor(
            serviceType: serviceType,
            factory: (sp) =>
            {
                var bag = sp.GetRequiredService(bagType);
                var printer = ActivatorUtilities.CreateInstance(sp, typeof(MessagePrinter), [bag]);
                return ActivatorUtilities.CreateInstance(sp, serviceType, [printer]);
            },
            lifetime: ServiceLifetime.Transient
        );

        services.TryAdd(serviceDescriptor);


        return services;
    }
}

public class DefaultMessageFactory
{

}