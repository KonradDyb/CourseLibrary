using CourseLibrary.API.ValidationAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    // Custom attributes are executed before the validate method gets called,
    // and that can come in handy for property-level validation.
    // Yet at class level, the same rules still apply.
    // If property-level validation fails, class-level validation
    // will not occur even when using custom attributes.
    [CourseTitleMustBeDifferentFromDescription]
    public class CourseForCreationDto /*: IValidatableObject*/
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

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
