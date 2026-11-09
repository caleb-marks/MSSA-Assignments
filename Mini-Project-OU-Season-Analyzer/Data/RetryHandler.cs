using System.Net;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Download retries: maximum four attempts
internal sealed class RetryHandler : DelegatingHandler
{
    private const int Tries = 4;
    // Maximum requested retry delay
    private static readonly TimeSpan MostWait = TimeSpan.FromSeconds(10);

    // Enable automatic decompression
    public RetryHandler() : base(new SocketsHttpHandler { AutomaticDecompression = DecompressionMethods.All })
    {
    }

    // Retry transient download failures
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancel)
    {
        for (int attempt = 1; ; attempt++)
        {
            TimeSpan wait = TimeSpan.FromMilliseconds(500 * attempt);
            try
            {
                HttpResponseMessage response = await base.SendAsync(request, cancel).ConfigureAwait(false);
                if (attempt == Tries || !IsBusy(response.StatusCode))
                {
                    return response;
                }
                // Cap requested retry delay
                TimeSpan? asked = response.Headers.RetryAfter != null ? response.Headers.RetryAfter.Delta : null;
                if (asked.HasValue)
                {
                    wait = asked.Value < MostWait ? asked.Value : MostWait;
                }
                response.Dispose();
            }
            catch (HttpRequestException) when (attempt < Tries && !cancel.IsCancellationRequested)
            {
                // Retry dropped connection
            }
            await Task.Delay(wait, cancel).ConfigureAwait(false);
        }
    }

    // Check retryable status codes
    private static bool IsBusy(HttpStatusCode code) =>
        code == HttpStatusCode.TooManyRequests || code == HttpStatusCode.BadGateway || code == HttpStatusCode.ServiceUnavailable
        || code == HttpStatusCode.GatewayTimeout;

    // Read deepest error message
    public static string Reason(Exception error)
    {
        while (error.InnerException != null)
        {
            error = error.InnerException;
        }
        return error.Message;
    }
}
