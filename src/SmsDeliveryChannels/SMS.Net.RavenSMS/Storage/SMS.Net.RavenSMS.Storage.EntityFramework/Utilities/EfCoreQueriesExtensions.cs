namespace SMS.Net.Channel.RavenSMS.Stores.EntityFramework;

internal static class EfCoreQueriesExtensions
{
    /// <summary>
    /// build the OrderBy Query dynamically
    /// </summary>
    /// <typeparam name="T">the type of entity we building the orderBy for it</typeparam>
    /// <param name="query">the query it self</param>
    /// <param name="sortColumn">the column you are soring with it</param>
    /// <param name="SortDirection">is the sorting direction</param>
    /// <returns>the query</returns>
    public static IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> query, string sortColumn, SortDirection SortDirection)
    {
        // Dynamically creates a call like this: query.OrderBy(p =&gt; p.SortColumn)
        var parameter = Expression.Parameter(typeof(T), "p");

        // get the object type
        var objType = typeof(T);

        var property = objType.GetProperty(sortColumn,
            BindingFlags.Public |
            BindingFlags.Instance |
            BindingFlags.IgnoreCase |
            BindingFlags.FlattenHierarchy);

        if (property is null)
            property = objType.GetProperty("Id");

        // no property has been found
        if (property is null)
            return query;

        // this is the part p.SortColumn
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);

        // this is the part p =&gt; p.SortColumn
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        // the sorting command
        var orderByCommand = SortDirection == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";

        // finally, call the "OrderBy" / "OrderByDescending" method with the order by lambda expression
        if (query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable),
            orderByCommand, new Type[] { objType, property.PropertyType }, query.Expression,
            Expression.Quote(orderByExpression))) is not IOrderedQueryable<T> orderQueury)
        {
            return query;
        }

        return orderQueury;
    }
}

