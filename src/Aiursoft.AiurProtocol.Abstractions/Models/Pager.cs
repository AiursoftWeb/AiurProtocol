using System.ComponentModel.DataAnnotations;

namespace Aiursoft.AiurProtocol.Interfaces;

public abstract class Pager
{
    /// <summary>
    /// How many items should there include in one page. Max is 100.
    /// </summary>
    [Range(1, 100)]
    [Required(ErrorMessage = "Please provide the page size!")]
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// The page number of the grouped items. Starts with 1.
    /// </summary>
    [Range(1, int.MaxValue)]
    [Required(ErrorMessage = "Please provide the page number!")]
    public int PageNumber { get; init; } = 1;
}