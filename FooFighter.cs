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
