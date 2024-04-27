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
