﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : (int?)null;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int? pageNumber = 0, int? pageSize = 0)
        {
            var pNumber = (int)(pageNumber == null || pageNumber == 0 ? 1 : pageNumber);
            var pSize = (int)(pageSize == null || pageSize == 0 ? source.Count() : pageSize);

            var count = source.Count();
            var items = source.Skip((pNumber - 1) * pSize).Take(pSize).ToList();

            return new PagedList<T>(items, count, pNumber, pSize);
        }
    }
}
