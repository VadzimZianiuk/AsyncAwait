using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static async Task<long> CalculateAsync(int n, CancellationToken token)
    {
        long sum = n;
        for (var i = 1; i < n; i++)
        {
            sum += i;
            await Task.Delay(10, token).ConfigureAwait(false);
        }

        return sum;
    }
}
