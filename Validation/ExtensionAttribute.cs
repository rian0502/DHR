using System.ComponentModel.DataAnnotations;

namespace DHR.Validation;

public class ExtensionAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public ExtensionAttribute(string extensions)
    {
        _extensions = extensions.Split(",").Select(ext => ext.Trim().ToLowerInvariant())
            .Where(ext => ext.StartsWith(".")).ToArray();
    }

    public override bool IsValid(object? value)
    {
        if (value is not IFormFile file)
        {
            return true;
        }
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _extensions.Contains(extension);
    }

    public override string FormatErrorMessage(string name)
    {
        var extensions = string.Join(", ", _extensions);
        return $"The file {name} must have one of the following extensions: {extensions}.";
    }
}