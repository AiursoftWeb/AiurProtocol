using System;
using System.Collections.Generic;

namespace Aiursoft.AiurProtocol.Models;

public class AiurCollection<T> : AiurResponse
{
    [Obsolete("This method is only for framework", true)]
    public AiurCollection()
    {
    }

    public AiurCollection(List<T> items)
    {
        Items = items;
    }

    public List<T>? Items { get; set; }
}