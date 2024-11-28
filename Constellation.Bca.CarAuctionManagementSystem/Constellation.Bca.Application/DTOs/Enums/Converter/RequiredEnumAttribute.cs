
using System.ComponentModel.DataAnnotations;

namespace Constellation.Bca.Application.DTOs.Enums.Converter
{
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(Convert.ToString(value))) return false;

            return base.IsValid(value);
        }
    }
}
