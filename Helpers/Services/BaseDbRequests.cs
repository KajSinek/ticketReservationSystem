using Helpers.Models;
using Helpers.Resources;
using Helpers.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Helpers.Services;

public class BaseDbRequests<TContext> : IBaseDbRequests
    where TContext : DbContext
{

    private readonly TContext _context;

    public BaseDbRequests(TContext context)
    {
        _context = context;
    }

    public DbSet<T> Set<T>() where T : class
    {
        return _context.Set<T>();
    }

    public async Task<EntityResponse<T>> CreateAsync<T>(T entity) where T : class
    {
        var data = await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();

        return new EntityResponse<T>
        {
            Entity = data.Entity
        };
    }

    public async Task<EntityResponse<T>> UpdateAsync<T>(Guid id, T entity) where T : class
    {
        var foundEntity = GetAsync<T>(id);
        if (foundEntity.Result.Errors.Count > 0)
            return foundEntity.Result;
        var data = _context.Set<T>().Update(entity);
        if (!entity.Equals(foundEntity.Result.Entity))
            await _context.SaveChangesAsync();

        return new EntityResponse<T>
        {
            Entity = data.Entity
        };
    }

    public Task<Response> DeleteAsync<T>(Guid id) where T : class
    {
        var data = GetAsync<T>(id);
        var response = new Response();
        if (data.Result.Entity == null)
        {
            response.Errors.Add(string.Format(ErrorMessages.EntityNotFound, typeof(T).ToString(), id));
            response.StatusCode = HttpStatusCode.NotFound;
            return Task.FromResult(response);
        }
        _context.Set<T>().Remove(data.Result.Entity);
        _context.SaveChanges();
        return Task.FromResult(response);
    }

    public Task<EntitiesResponse<T>> GetAllAsync<T>() where T : class
    {
        var data = _context.Set<T>().ToList();
        return Task.FromResult(new EntitiesResponse<T>
        {
            Entities = data
        });
    }

    public async Task<EntityResponse<T>> GetAsync<T>(object keyValues) where T : class
    {
        var entityType = typeof(T);
        var primaryKeyProperties = _context.Model.FindEntityType(entityType).FindPrimaryKey().Properties;

        if (!primaryKeyProperties.Any())
        {
            throw new InvalidOperationException($"Entity type {entityType.Name} does not have a primary key");
        }

        // Ensure the keyValues argument is valid
        var keyValuesArray = keyValues as object[];
        if (primaryKeyProperties.Count > 1 && (keyValuesArray == null || keyValuesArray.Length != primaryKeyProperties.Count))
        {
            throw new ArgumentException($"Invalid key values for composite primary key of {entityType.Name}");
        }

        // Build the filter expression dynamically for composite or single key
        var parameter = Expression.Parameter(entityType, "e");
        Expression combinedFilter = null;

        for (int i = 0; i < primaryKeyProperties.Count; i++)
        {
            var propertyAccess = Expression.MakeMemberAccess(parameter, primaryKeyProperties[i].PropertyInfo);
            var keyValue = primaryKeyProperties.Count == 1 ? keyValues : keyValuesArray[i];
            var equality = Expression.Equal(propertyAccess, Expression.Constant(keyValue));
            combinedFilter = combinedFilter == null ? equality : Expression.AndAlso(combinedFilter, equality);
        }

        var filter = Expression.Lambda<Func<T, bool>>(combinedFilter, parameter);

        var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter);
        if (entity == null)
        {
            return new EntityResponse<T>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, typeof(T).ToString(), string.Join(", ", keyValuesArray ?? new[] { keyValues })) },
                StatusCode = HttpStatusCode.NotFound
            };
        }

        return new EntityResponse<T> { Entity = entity };
    }

    public async Task<EntityResponse<T>> GetEntityByPropertyAsync<T>(GetEntityByPropertyModel<T> model) where T : class
    {
        var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(model.Query);
        if (entity == null)
        {
            return new EntityResponse<T>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, typeof(T).ToString(), model.Query) },
                StatusCode = HttpStatusCode.NotFound
            };
        }

        return new EntityResponse<T> { Entity = entity };
    }
}
