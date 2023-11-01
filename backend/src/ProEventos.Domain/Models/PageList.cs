using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList()
        {
            
        }

        public PageList(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalCount = totalCount;
            PageSize = pageSize;
            TotalPages = (int) Math.Ceiling(TotalPages / (double) pageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source
                .Skip((currentPage-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<T>(items, totalCount, currentPage, pageSize);
        }

    }
}
