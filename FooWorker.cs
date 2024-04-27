public class FooWorker : IFooWorker
{
    private readonly IMessagePrinter _messagePrinter;

    public FooWorker(IMessagePrinter messagePrinter)
    {
        _messagePrinter = messagePrinter;
    }

    public string DoWork()
    {
        return _messagePrinter.Print("DoWork");
    }
}
