public class MessagePrinterConfiguration
{
    public Type? MessageBagType { get; private set; }

    public void UseMassageBag<TBag>() where TBag : class, IMessageBag
    {
        MessageBagType = typeof(TBag);
    }
}