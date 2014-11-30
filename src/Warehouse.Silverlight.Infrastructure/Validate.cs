using System.Collections.Generic;

namespace Warehouse.Silverlight.Infrastructure
{
    public static class Validate
    {
        public static IEnumerable<string> Required(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield return "обязательное поле";
            }
        }

        public static IEnumerable<string> Password(string password, string password2)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                yield return "обязательное поле";
                yield break;
            }
            if (!string.Equals(password, password2))
            {
                yield return "новый пароль повторен неправильно";
                yield break;
            }
            if (password.Length < 6)
            {
                yield return "пароль должен быть не менее 6 символов в длину";
            }
        }

        public static IEnumerable<string> Double(string value)
        {
            double d;
            if (!double.TryParse(value, out d))
            {
                yield return "дробное число";
            }
        }

        public static IEnumerable<string> Long(string value)
        {
            long l;
            if (!long.TryParse(value, out l))
            {
                yield return "целое число";
            }
        }

        public static IEnumerable<string> Int(string value)
        {
            int i;
            if (!int.TryParse(value, out i))
            {
                yield return "целое число";
            }
        }
    }
}
