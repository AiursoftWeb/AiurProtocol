using Aiursoft.AiurProtocol.Interfaces;
using Aiursoft.AiurProtocol.Models;
using Microsoft.EntityFrameworkCore;

namespace Aiursoft.AiurProtocol.Services
{
    public static class AiurPagedCollectionBuilder
    {
        public static async Task<AiurPagedCollection<T>> BuildAsync<T>(
            IOrderedQueryable<T> query,
            IPageable pager,
            ErrorType code,
            string message)
        {
            var items = await query.Page(pager).ToListAsync();
            return new AiurPagedCollection<T>(items)
            {
                TotalCount = await query.CountAsync(),
                CurrentPage = pager.PageNumber,
                CurrentPageSize = pager.PageSize,
                Code = code,
                Message = message
            };
        }

        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> query, IPageable pager)
        {
            return query
                .Skip((pager.PageNumber - 1) * pager.PageSize)
                .Take(pager.PageSize);
        }
    }
}
