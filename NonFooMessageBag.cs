public class NonFooMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"NonFoo message for: {key}";
    }
}
