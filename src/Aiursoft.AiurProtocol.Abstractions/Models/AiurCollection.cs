namespace Aiursoft.AiurProtocol.Models;

public class AiurCollection<T> : AiurResponse
{
    [Obsolete("This method is only for framework", true)]
    public AiurCollection()
    {
    }

    public AiurCollection(IReadOnlyCollection<T> items)
    {
        Items = items;
    }

    public IReadOnlyCollection<T>? Items { get; set; }
}