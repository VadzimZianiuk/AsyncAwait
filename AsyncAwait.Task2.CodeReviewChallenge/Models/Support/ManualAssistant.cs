using CloudServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support;

public class ManualAssistant : IAssistant
{
    private readonly ISupportService _supportService;
    private readonly ILogger<ManualAssistant> _logger;

    public ManualAssistant(ISupportService supportService, ILogger<ManualAssistant> logger)
    {
        _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        _logger = logger;
    }

    public async Task<string> RequestAssistanceAsync(string requestInfo)
    {
        try
        {
            await _supportService.RegisterSupportRequestAsync(requestInfo).ConfigureAwait(false);
            return await _supportService.GetSupportInfoAsync(requestInfo).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to register assistance request.");
            return $"Failed to register assistance request. Please try later. {ex.Message}";
        }
    }
}
