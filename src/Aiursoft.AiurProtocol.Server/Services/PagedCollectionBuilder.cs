using Aiursoft.AiurProtocol.Interfaces;
using Aiursoft.AiurProtocol.Models;
using Microsoft.EntityFrameworkCore;

namespace Aiursoft.AiurProtocol.Server.Services
{
    public static class AiurPagedCollectionBuilder
    {
        public static async Task<AiurPagedCollection<T>> BuildAsync<T>(
            IOrderedQueryable<T> query,
            Pager pager,
            Code code,
            string message)
        {
            List<T> items;
            int count;
            if (query is IAsyncEnumerable<T>)
            {
                items = await query.Page(pager).ToListAsync();
                count = await query.CountAsync();
            }
            else
            {
                items = query.Page(pager).ToList();
                count = query.Count();
            }
            return new AiurPagedCollection<T>(items)
            {
                TotalCount = count,
                CurrentPage = pager.PageNumber,
                CurrentPageSize = pager.PageSize,
                Code = code,
                Message = message
            };
        }

        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> query, Pager pager)
        {
            return query
                .Skip((pager.PageNumber - 1) * pager.PageSize)
                .Take(pager.PageSize);
        }
    }
}
