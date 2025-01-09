using Helpers.Models;
using Helpers.Responses;
using Microsoft.EntityFrameworkCore;

namespace Helpers.Services;

public interface IBaseDbRequests
{
    public Task<EntityResponse<T>> CreateAsync<T>(T entity) where T : class;
    public Task<EntityResponse<T>> UpdateAsync<T>(T entity, params object[] keyValues) where T : class;
    public Task<EntityResponse<T>> UpdateAsync<T>(T entity, Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null, params object[] keyValues) where T : class;
    public Task<Response> DeleteAsync<T>(params object[] keyValues) where T : class;
    public Task<Response> DeleteAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null, params object[] keyValues) where T : class;
    public Task<EntityResponse<T>> GetAsync<T>(object keyValues, Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class;
    public Task<EntitiesResponse<T>> GetAllAsync<T>(Func<IQueryable<T>, IQueryable<T>> queryConfigurator = null) where T : class;
    public Task<EntityResponse<T>> GetEntityByPropertyAsync<T>(GetEntityByPropertyModel<T> model) where T : class;
    public DbSet<T> Set<T>() where T : class;
}
