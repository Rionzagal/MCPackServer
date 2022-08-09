using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MCPackServer.Shared
{
    public static class Validations
    {
        public static IEnumerable<string> ValidateEmail(string input)
        {
            if (!string.IsNullOrEmpty(input) && !Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                yield return "El campo debe de tener la siguente estructura: 'contoso@domain.com'.";
        }

        public static IEnumerable<string> ValidatePhoneNumber(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Any(ch => char.IsLetter(ch)))
                    yield return "El campo no admite caracteres alfabéticos.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    yield return "El campo no admite caracteres especiales.";
            }
        }

        public static IEnumerable<string> ValidateNumericCode(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Any(ch => !char.IsLetter(ch)))
                    yield return "El campo no acepta caracteres alfabéticos.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    yield return "El campo no acepta caracteres especiales.";
            }
        }

        public static IEnumerable<string> ValidateUniqueNumericCode(string input, IEnumerable<string> TakenFields)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Any(ch => char.IsLetter(ch)))
                    yield return "El campo no acepta caracteres alfabéticos.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    yield return "El campo no acepta caracteres especiales.";
                if (TakenFields.Any(f => f == input))
                    yield return "Ya existe un registro con el valor asignado en el campo.";
            }
        }

        public static IEnumerable<string> ValidateAlphaNumericCode(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (Regex.IsMatch(input, "[a-z]"))
                    yield return "El campo no admite letras minúsculas.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    yield return "El código no debe contener caracteres especiales.";
            }
        }

        public static IEnumerable<string> ValidateUniqueAlphaNumericCode(string input, IEnumerable<string> TakenFields)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (Regex.IsMatch(input, "[a-z]"))
                    yield return "El campo no admite letras minúsculas.";
                if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    yield return "El código no debe contener caracteres especiales.";
                if (TakenFields.Any(f => f == input))
                    yield return "Ya existe un registro con este valor.";
            }
        }

        public static IEnumerable<string> ValidateUrl(string input, int maxLength)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Any(ch => char.IsWhiteSpace(ch)))
                    yield return "El campo no acepta espacios en blanco.";
                if (!Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
                    yield return "La url introducida no está bien formada.";
                if (maxLength < input.Length)
                    yield return $"El campo tiene un máximo de {maxLength} caracteres.";
            }
        }

        public static string ValidateLength(string input, int maxLength)
        {
            string message = string.Empty;
            if (!string.IsNullOrEmpty(input) && maxLength < input.Length)
                message = $"El campo tiene un máximo de {maxLength} caracteres.";
            return message;
        }

        public static IEnumerable<string> ValidatePassword(string input)
        {
            if (8 > input.Length)
                yield return "La contraseña debe de tener al menos 8 caracteres.";
            if (!Regex.IsMatch(input, @"[A-Z]"))
                yield return "La contraseña debe tener al menos una letra mayúscula.";
            if (!Regex.IsMatch(input, @"[a-z]"))
                yield return "La contraseña debe tener al menos una letra minúscula.";
            if (!Regex.IsMatch(input, @"[0-9]"))
                yield return "La contraseña debe tener al menos un número.";
            if (!input.Any(ch => !char.IsLetterOrDigit(ch)))
                yield return "La contraseña debe tener al menos un caracter especial.";
        }

        public static IEnumerable<string> PasswordMatch(string input, string reference)
        {
            if (!string.Equals(input, reference))
            {
                yield return "Las contraseñas no coinciden.";
                yield break;
            }
        }

        public static int? GetMaxLengthForProperty(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName);
            if (null == property)
                throw new ArgumentException("The given property name does not match with any property of the calling object.");
            else if (typeof(string).Name != property.PropertyType.Name)
                throw new ArgumentException("The given property name must refer to a string!");
            var attributes = (StringLengthAttribute[])property
                .GetCustomAttributes(typeof(StringLengthAttribute), false) ??
                new StringLengthAttribute[0];
            if (attributes.Any())
                return attributes.FirstOrDefault()?.MaximumLength;
            return null;
        }
    }
}