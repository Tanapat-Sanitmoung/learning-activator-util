using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMessagePrinter, MessagePrinter>();
builder.Services.AddTransient<IMessageBag, DefaultMessageBag>();

builder.Services.AddTransient<FooFighterFactory>();
builder.Services.AddTransient<FooMessageBag>();

builder.Services.AddTransient(sp => {
    var factory = sp.GetRequiredService<FooFighterFactory>();
    return factory.Create();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/typed", ([FromServices]FooFighter fooFighter) =>
{
    return fooFighter.DoMagic();
    //Output: Custom message for: Magic
})
.WithName("typed")
.WithOpenApi();

app.MapGet("/default", ([FromServices]IMessagePrinter printer) => {

    return printer.Print("Magic");
    //Output: Default message for: Magic
})
.WithName("default")
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
        return $"Custom message for: {key}";
    }
}