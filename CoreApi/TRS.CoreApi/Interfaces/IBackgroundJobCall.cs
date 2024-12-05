using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace TRS.CoreApi.Interfaces;

public interface IBackgroundJobCall
{
    [Get("/api/backgroundJobTest")]
    Task<EntityResponse<string>> GetDataAsync();
}
