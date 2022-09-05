
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Librarymanagement.Models
{
    public class Book
    {
        public int Book_Id { get; set; } = 0;

        [Required(ErrorMessage = "The Field {0} is required")]
        public string? BookName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Field {0} is required")]
        public int MRP { get; set; } = 0;
        [Required(ErrorMessage = "The Field {0} is required")]


        public string? Language { get; set; } = string.Empty;
        // [Required(ErrorMessage = "The Field {0} is required")]

        public string? AuthorName { get; set; } = string.Empty;
        // [Required(ErrorMessage = "The Field {0} is required")]
        public string? PublisherName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Field {0} is required")]
        public DateTime Published_Date { get; set; }
        [Required(ErrorMessage = "The Field {0} is required")]
        public string? Volume { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Field {0} is required")]

        
        public int Language_id { get; set; } = 0;
        [Required(ErrorMessage = "The Field {0} is required")]
        public string? Status { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Field {0} is required")]
        public int Author_Id { get; set; } = 0;
        
        [Required(ErrorMessage = "The Field {0} is required")]

        public int Publisher_id { get; set; } = 0;
       

    }
}
