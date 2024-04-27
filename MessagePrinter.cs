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
