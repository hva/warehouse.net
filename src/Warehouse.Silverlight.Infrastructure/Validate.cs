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
    }
}
