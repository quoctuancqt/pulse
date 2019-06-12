namespace Pulse.Common.Helpers
{
    using System;
    using System.Linq.Expressions;

    public static class LambdaHelper
    {
        public static Expression<Func<TEntity, bool>> BuildQuery<TEntity>(string condition)
        {
            var c = condition.Split(new string[] { "==" }, StringSplitOptions.None);
            var propertyName = c[0].Trim();
            var value = c[1].Trim();

            var arg = Expression.Parameter(typeof(TEntity), "x");
            var property = typeof(TEntity).GetProperty(propertyName);
            var comparison = Expression.Equal(
                Expression.MakeMemberAccess(arg, property),
                Expression.Constant(value));

            //var lambda = Expression.Lambda<Func<TEntity, bool>>(comparison, arg).Compile();
            
            return Expression.Lambda<Func<TEntity, bool>>(comparison, arg);
        }
    }
}
