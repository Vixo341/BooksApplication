using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksApplication.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage ="Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
