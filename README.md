by looking at this code you will see understand it easily.

Initial:

```csharp
class Fooer
{
    public void DoItsJob()
    {
        Console.WriteLine("It is my duty");
    }
}

class IFooFighter
{
    private readonly Fooer _fooer;

    public IFooFighter(int a, string b, bool c, string d, Fooer fooer)
    {
        Console.WriteLine("a => {0}", a);
        Console.WriteLine("b => {0}", b);
        Console.WriteLine("c => {0}", c);
        Console.WriteLine("d => {0}", d);
        _fooer = fooer;
    }
    
    public void CallFooer()
    {
        _fooer.DoItsJob();
    }
}
```

Usage and output:

```csharp
app.MapGet("/test", (IServiceProvider services) =>
{
    var fighter = (IFooFighter)ActivatorUtilities.CreateInstance(
        services, typeof(IFooFighter), ["hi", 1, "there", true]);

        fighter.CallFooer();

        /*
        OUTPUT:
            a => 1
            b => hi
            c => True
            d => there
            It is my duty
        */
})
.WithName("test")
.WithOpenApi();
```
