using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.ViewModel;

namespace Warehouse.Silverlight.Infrastructure
{
    public static class Extensions
    {
        public static bool HasErrors<T>(this ErrorsContainer<T> container, params Expression<Func<T>>[] propertyExpressions)
        {
            foreach (var propertyExpression in propertyExpressions)
            {
                var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
                if (container.GetErrors(propertyName).Any())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
