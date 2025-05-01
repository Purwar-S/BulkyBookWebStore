using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Range(1,1000, ErrorMessage = "Range should be between 1 to 1000"), DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
