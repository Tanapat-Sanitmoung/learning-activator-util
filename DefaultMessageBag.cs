public class DefaultMessageBag : IMessageBag
{
    public string Get(string key)
    {
        return $"Default message for: {key}";
    }
}
