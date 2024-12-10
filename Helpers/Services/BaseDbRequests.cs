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

    public async Task<EntityResponse<T>> UpdateAsync<T>(
    Guid id,
    T entity,
    Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class
    {
        // Fetch the entity using query configurator
        var query = _context.Set<T>().AsQueryable();
        if (queryConfigurator != null)
        {
            query = queryConfigurator(query); // Apply includes or other query configurations
        }

        var foundEntity = await query.FirstOrDefaultAsync(e =>
            EF.Property<Guid>(e, "Id") == id); // Replace "Id" with the actual primary key property if different

        if (foundEntity == null)
        {
            return new EntityResponse<T>
            {
                Errors = new List<string> { "Entity not found." },
                StatusCode = HttpStatusCode.NotFound
            };
        }

        // Update the entity
        _context.Entry(foundEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();

        // Re-fetch the updated entity with the query configurator applied
        var updatedQuery = _context.Set<T>().AsNoTracking(); // No tracking for the response
        if (queryConfigurator != null)
        {
            updatedQuery = queryConfigurator(updatedQuery); // Reapply the includes
        }

        var updatedEntity = await updatedQuery.FirstOrDefaultAsync(e =>
            EF.Property<Guid>(e, "Id") == id); // Replace "Id" with your primary key property name

        if (updatedEntity == null)
        {
            return new EntityResponse<T>
            {
                Errors = new List<string> { "Updated entity could not be retrieved." },
                StatusCode = HttpStatusCode.NotFound
            };
        }

        return new EntityResponse<T>
        {
            Entity = updatedEntity
        };
    }

    public Task<Response> DeleteAsync<T>(Guid id, Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class
    {
        var data = GetAsync<T>(id, queryConfigurator);
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
