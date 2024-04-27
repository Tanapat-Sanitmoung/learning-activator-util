public class FooMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"Foo message for: {key}";
    }
}
