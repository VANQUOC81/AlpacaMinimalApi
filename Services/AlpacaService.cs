using Alpaca.Markets;

namespace Services
{
    /// <summary>
    /// Alpaca Service
    /// </summary>
    public class AlpacaService
    {
        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> GetClock()
        {
            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            var clock = await client.GetClockAsync();

            if (clock != null)
            {
                return TypedResults.Ok(clock);
            }

            return TypedResults.NotFound();
        }
    }
}
