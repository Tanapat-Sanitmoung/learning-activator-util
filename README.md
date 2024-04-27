Add Dependency Inkection Extension methods:

Quick and Simple way to build the methods (I will refacor them later)


Register service:

```csharp
builder.Services.AddMessagePrinter();
builder.Services.AddMessagePrinter<FooFighter>(() => typeof(FooMessageBag));
builder.Services.AddMessagePrinter<NonFooFighter>(() => typeof(NonFooMessageBag));
```

Usage:

```csharp
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
```

I'm feeling better now :)
