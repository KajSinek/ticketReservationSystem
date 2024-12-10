namespace Helpers.Responses;

public static class ListDtoResponse
{
    public static EntitiesResponse<TDto> ToCollectionDto<TEntity, TDto>(this IEnumerable<TEntity> list, Func<TEntity, TDto> expression)
        => new() { Entities = list.Select(expression).ToList() };
}
