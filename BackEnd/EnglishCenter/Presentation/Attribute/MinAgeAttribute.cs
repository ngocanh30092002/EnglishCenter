using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Attribute
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private int _minAge;
        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;

            ErrorMessage = $"Age must be at least {_minAge} years old.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateOnly dateOfBirth)
            {
                var age = DateTime.Today.Year - dateOfBirth.Year;

                if (dateOfBirth.ToDateTime(TimeOnly.MinValue) > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                if (age >= _minAge)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
