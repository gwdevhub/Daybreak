namespace Daybreak.Validators;
public sealed class BooleanValidator : IValidator
{
    public bool IsValid(object value)
    {
        if (value is string stringValue &&
            bool.TryParse(stringValue, out _))
        {
            return true;
        }
        else if (value is bool)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
