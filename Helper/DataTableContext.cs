using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DHR.Helper
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? SearchValue { get; set; }
        public int SortColumnIndex { get; set; }
        public string? SortColumnDirection { get; set; }
    }

    public class DataTableResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class DataTableContext
    {
        public async Task<DataTableResponse<T>> ProcessRequest<T>(
            IQueryable<T> query, 
            DataTableRequest request, 
            string[] columnNames)
        {
            // 1. Searching
            if (!string.IsNullOrEmpty(request.SearchValue))
            {
                query = query.Where(item =>
                    columnNames.Any(col => EF.Property<string>(item, col).Contains(request.SearchValue))
                );
            }

            // 2. Total record count setelah filtering
            var totalRecords = await query.CountAsync();

            // 3. Sorting
            if (request.SortColumnIndex >= 0 && request.SortColumnIndex < columnNames.Length)
            {
                string sortColumn = columnNames[request.SortColumnIndex];
                query = request.SortColumnDirection == "asc"
                    ? query.OrderBy(x => EF.Property<object>(x, sortColumn))
                    : query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
            }

            // 4. Pagination
            var pagedData = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ToListAsync();

            return new DataTableResponse<T>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = pagedData
            };
        }
    }
}
