This will demonstrate the result of select DefaultMessageBag and FooMessageBag.

`FooFighterFactory` will create an instance of FooMessageBag to intead of let service provider resolve it.

```csharp
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
```


`DefaultMessageBag` will be register there for non-typed client.

```csharp
builder.Services.AddTransient<IMessagePrinter, MessagePrinter>();
builder.Services.AddTransient<IMessageBag, DefaultMessageBag>();

builder.Services.AddTransient<FooFighterFactory>();
builder.Services.AddTransient<FooMessageBag>();

builder.Services.AddTransient(sp => {
    var factory = sp.GetRequiredService<FooFighterFactory>();
    return factory.Create();
});
```

Let's run, execute and see it for yourself
