using System.ComponentModel.DataAnnotations;

namespace Aiursoft.AiurProtocol.Interfaces;

public interface IPager
{
    /// <summary>
    ///     Default is 10
    /// </summary>
    [Range(1, 100)]
    public int PageSize { get; set; }

    /// <summary>
    ///     Starts from 1.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }
}