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

    public Task<EntityResponse<T>> UpdateAsync<T>(T entity, params object[] keyValues) where T : class
    {
        return UpdateAsync(entity, null, keyValues);
    }

    public async Task<EntityResponse<T>> UpdateAsync<T>(
    T entity,
    Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null,
    params object[] keyValues) where T : class
    {
        var response = new EntityResponse<T>();

        // Ensure keyValues are provided
        if (keyValues == null || keyValues.Length == 0)
        {
            response.Errors.Add("Key values are required.");
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }

        // Retrieve the entity using composite keys
        var foundEntity = await _context.Set<T>().FindAsync(keyValues);

        if (foundEntity == null)
        {
            response.Errors.Add("Entity not found.");
            response.StatusCode = HttpStatusCode.NotFound;
            return response;
        }

        // Update the entity
        _context.Entry(foundEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();

        // Re-fetch the updated entity with query configurator applied (optional)
        var updatedQuery = _context.Set<T>().AsNoTracking();
        if (queryConfigurator != null)
        {
            updatedQuery = queryConfigurator(updatedQuery);
        }

        // Retrieve primary key property names dynamically
        var keyProperties = _context.Model.FindEntityType(typeof(T))
                              ?.FindPrimaryKey()
                              ?.Properties
                              .Select(p => p.Name)
                              .ToArray();

        if (keyProperties == null || keyProperties.Length != keyValues.Length)
        {
            response.Errors.Add("Mismatch between key values and entity's primary key properties.");
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }

        // Build the composite key condition dynamically
        var parameter = Expression.Parameter(typeof(T), "e");
        Expression compositeCondition = Expression.Constant(true); // Start with a "true" condition

        for (int i = 0; i < keyProperties.Length; i++)
        {
            var propertyExpression = Expression.Property(parameter, keyProperties[i]);
            var keyValueExpression = Expression.Constant(keyValues[i]);
            var equalsExpression = Expression.Equal(propertyExpression, keyValueExpression);

            compositeCondition = Expression.AndAlso(compositeCondition, equalsExpression);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(compositeCondition, parameter);

        // Apply the lambda expression
        var updatedEntity = await updatedQuery.FirstOrDefaultAsync(lambda);

        if (updatedEntity == null)
        {
            response.Errors.Add("Failed to retrieve the updated entity.");
            response.StatusCode = HttpStatusCode.NotFound;
            return response;
        }

        response.Entity = updatedEntity;
        return response;
    }

    public Task<Response> DeleteAsync<T>(params object[] keyValues) where T : class
    {
        return DeleteAsync<T>(null, keyValues);
    }

    public async Task<Response> DeleteAsync<T>(
    Func<IQueryable<T>, IQueryable<T>> queryConfigurator,
    params object[] keyValues) where T : class
    {
        var response = new Response();

        // Ensure keyValues are not null or empty
        if (keyValues == null || keyValues.Length == 0)
        {
            response.Errors.Add("Key values are required.");
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }

        // Retrieve the entity using composite keys
        var entity = await _context.Set<T>().FindAsync(keyValues);

        if (entity == null)
        {
            response.Errors.Add(string.Format(ErrorMessages.EntityNotFound, typeof(T).Name, string.Join(", ", keyValues)));
            response.StatusCode = HttpStatusCode.NotFound;
            return response;
        }

        // Remove the entity
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();

        return response;
    }

    public async Task<EntitiesResponse<T>> GetAllAsync<T>(
    Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class
    {
        // Start with the base query
        IQueryable<T> query = _context.Set<T>().AsNoTracking();

        // Apply the query configurator if provided
        if (queryConfigurator != null)
        {
            query = queryConfigurator(query);
        }

        // Execute the query and fetch the data
        var data = await query.ToListAsync();

        // Return the response
        return new EntitiesResponse<T>
        {
            Entities = data
        };
    }


    public async Task<EntityResponse<T>> GetAsync<T>(
    object keyValues,
    Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class
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

        // Apply the query configuration if provided
        IQueryable<T> query = _context.Set<T>().AsNoTracking();
        if (queryConfigurator != null)
        {
            query = queryConfigurator(query);
        }

        var entity = await query.FirstOrDefaultAsync(filter);
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
