using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Librarymanagement.Models
{
    public class GetBook
    {
        public int Book_Id { get; set; } = 0;
        public string? BookName { get; set; } = string.Empty;
         public int MRP { get; set; } = 0;
         public string? Language { get; set; } = string.Empty;
       

        public string? AuthorName { get; set; } = string.Empty;
        
        public string? PublisherName { get; set; } = string.Empty;
       
        public DateTime Published_Date { get; set; }
        
        public string? Volume { get; set; } = string.Empty;
        


    }
}