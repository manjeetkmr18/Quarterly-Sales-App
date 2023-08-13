using System;
using System.Linq;
using System.Linq.Expressions;

namespace Quarterly_Sales_App.Models
{
    public static class PredicateExtend
    {
        /// <summary>
        /// Predicate not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)

        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);

        }
        /// <summary>
        /// Predicate And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)

        {
            var invoked = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, invoked), left.Parameters);
        }
        /// <summary>
        /// Predicate or
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)

        {
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, right.Body), left.Parameters);
        }
    }
}

