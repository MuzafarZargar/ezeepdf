using System.ComponentModel.DataAnnotations;

namespace EzeePdf.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringSameAttribute : ValidationAttribute
    {
        private readonly string otherProperty;
        public StringSameAttribute(string otherProperty, string errorMessage)
        {
            this.otherProperty = otherProperty;
            ErrorMessage = errorMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {otherProperty}");
            }
            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            // If both values are null, that’s fine
            if (value is null && otherValue is null)
            {
                return ValidationResult.Success;
            }

            // Compare values
            if (value?.Equals(otherValue) == true)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ??
                $"{validationContext.DisplayName} must match {otherProperty}.");
        }
    }
}
