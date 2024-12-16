using System.ComponentModel.DataAnnotations;

namespace DHR.Validation;

public class MaxSizeAttribute(int maxSize) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not IFormFile file)
        {
            return true;
        }
        return file.Length <= maxSize;
    }
    
    public override string FormatErrorMessage(string name)
    {
        var maxSizeInKb = maxSize / 1024.0;
        return $"The file {name} must be less than or equal to {maxSizeInKb:N0} KB.";
    }
}