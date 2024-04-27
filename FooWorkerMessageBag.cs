public class FooWorkerMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"{nameof(FooWorkerMessageBag)} handle {key}";
    }
}
