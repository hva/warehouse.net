using System.Collections.Generic;

namespace Warehouse.Silverlight.MainModule.Infrastructure
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
    }
}
