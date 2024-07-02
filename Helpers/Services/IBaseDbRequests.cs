using Helpers.Models;
using Helpers.Responses;
using Microsoft.EntityFrameworkCore;

namespace Helpers.Services;

public interface IBaseDbRequests
{
    public Task<EntityResponse<T>> CreateAsync<T>(T entity) where T : class;
    public Task<EntityResponse<T>> UpdateAsync<T>(Guid id, T entity) where T : class;
    public Task<Response> DeleteAsync<T>(Guid id) where T : class;
    public Task<EntityResponse<T>> GetAsync<T>(Guid id) where T : class;
    public Task<EntitiesResponse<T>> GetAllAsync<T>() where T : class;
    public Task<EntityResponse<T>> GetEntityByPropertyAsync<T>(GetEntityByPropertyModel<T> model) where T : class;
    public DbSet<T> Set<T>() where T : class;
}
