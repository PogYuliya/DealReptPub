using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealRept.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DealRept.Models
{
    public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int CountRecords { get; set; }
        public SearchModel SearchModel { get; set; }
        public SortModel SortModel { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, SearchModel searchModel, SortModel sortModel)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            CountRecords = count;
            this.AddRange(items);
            SearchModel = searchModel;
            SortModel = sortModel;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T>source, int pageIndex, int pageSize, SearchModel searchModel, SortModel sortModel)
        {
             int count = await source.CountAsync();
           
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize,searchModel,sortModel);
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, SearchModel searchModel, SortModel sortModel)
        {
            int count = source.Count();

            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize, searchModel, sortModel);
        }


    }
}
