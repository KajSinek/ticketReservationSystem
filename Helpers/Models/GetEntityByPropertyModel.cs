using System.Linq.Expressions;

namespace Helpers.Models;

public class GetEntityByPropertyModel<T> where T : class
{
    public required Expression<Func<T, bool>> Query { get; set; }
}
