using System.Globalization;
using System.Windows.Controls;

namespace TravelPortal.ViewModels
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return int.TryParse((value ?? "").ToString(), NumberStyles.Integer, cultureInfo, out int number)
                ? new ValidationResult(false, "Field is integer.")
                : ValidationResult.ValidResult;

        }
    }
}