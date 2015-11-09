using System;
using System.Linq;
using System.Linq.Expressions;
using Prism.Mvvm;

namespace Warehouse.Wpf.UI.Modules
{
    public static class ValidationExtensions
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
