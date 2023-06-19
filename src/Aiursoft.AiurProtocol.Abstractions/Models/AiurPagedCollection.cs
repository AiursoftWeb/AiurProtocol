namespace Aiursoft.AiurProtocol.Models;

public class AiurPagedCollection<T> : AiurCollection<T>
{
    [Obsolete("This method is only for framework", true)]
    public AiurPagedCollection()
    {
    }

    public AiurPagedCollection(IReadOnlyCollection<T> items) : base(items)
    {
    }

    public int TotalCount { get; set; }

    /// <summary>
    ///     Starts from 1.
    /// </summary>
    public int CurrentPage { get; set; }

    public int CurrentPageSize { get; set; }
}