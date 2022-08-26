using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaginationFilter
{
    public class Pagination1
    {
       public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public Pagination1()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
    }
    public Pagination1(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize > 10 ? 10 : pageSize;
    }
    }
}