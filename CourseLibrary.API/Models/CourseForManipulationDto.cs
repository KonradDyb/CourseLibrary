using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{  
    // Custom attributes are executed before the validate method gets called,
    // and that can come in handy for property-level validation.
    // Yet at class level, the same rules still apply.
    // If property-level validation fails, class-level validation
    // will not occur even when using custom attributes.

    [CourseTitleMustBeDifferentFromDescription(
          ErrorMessage = "Title must be different from description")]
    public abstract class CourseForManipulationDto /*: IValidatableObject*/
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public virtual string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provided description should be different from the title.",
        //            new[] { "CourseForCreationDto" });
        //    }
        //}
    }
}
