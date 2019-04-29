using System.Globalization;
using System.Windows.Controls;

namespace TravelPortal.ViewModels
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Это обязательное поле.")
                : ValidationResult.ValidResult;
        }
    }
}