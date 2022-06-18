using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;
    private readonly ILogger<StatisticMiddleware> _logger;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService, ILogger<StatisticMiddleware> logger)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;
        try
        {
            await _statisticService.RegisterVisitAsync(path).ConfigureAwait(false);
            var visitors = await _statisticService.GetVisitsCountAsync(path).ConfigureAwait(false);
            UpdateHeaders(visitors);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Unable to register a visit to path: {path}");
        }

        await _next(context);

        void UpdateHeaders(long visitors)
        {
            context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, visitors.ToString());
        }
    }
}
