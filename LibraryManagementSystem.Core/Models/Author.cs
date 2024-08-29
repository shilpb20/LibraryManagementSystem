using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Core.Models
{
    public class Author : ModelBase
    {
        [Required]
        public string Name { get; set; }
    }
}
