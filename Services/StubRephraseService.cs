using CellarS.Api.Models;

namespace CellarS.Api.Services
{
    public class StubRephraseService : IRephraseService
    {
        public Task<RephraseResponse> RephraseTextAsync(RephraseRequest request)
        {
            var orignial = request.Text ?? string.Empty;

            var truncated = orignial.Length > 400 ? orignial.Substring(0, 400) + "..." : orignial;

            var simplified = $"[AI Simplified ({request.Mode ?? "simple"})]: {truncated}";

            var resp = new RephraseResponse
            {
                OriginalText = orignial,
                RephrasedText = simplified,
                Mode = request.Mode ?? "simple",
                Provider = "Stub",
                EstimatedLevel = null
            };

            return Task.FromResult(resp);
        }
    }
}