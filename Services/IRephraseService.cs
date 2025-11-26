using CellarS.Api.Models;

namespace CellarS.Api.Services
{
    public interface IRephraseService
    {
        Task<RephraseResponse> RephraseTextAsync(RephraseRequest request);
    }
}