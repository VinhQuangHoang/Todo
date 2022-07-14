using System.ComponentModel.DataAnnotations;

namespace Todolist.Models
{
    public class CreateEditTodoViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
