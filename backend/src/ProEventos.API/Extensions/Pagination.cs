using Microsoft.AspNetCore.Http;
using ProEventos.API.Models;
using ProEventos.Domain.Models;
using System.Text.Json;

namespace ProEventos.API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination<T>(this HttpResponse response, PageList<T> pageList)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var values = new PaginationHeader { 
                CurrentPage = pageList.CurrentPage, 
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(values, options));

            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

        }
    }
}
