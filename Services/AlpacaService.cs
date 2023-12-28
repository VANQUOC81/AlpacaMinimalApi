using Alpaca.Markets;

namespace Services
{
    /// <summary>
    /// Alpaca Service that gets information from Alpaca REST API endpoint.
    /// </summary>
    public class AlpacaService
    {
        /// <summary>
        /// Get current time information.
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

        /// <summary>
        /// Get our account information.
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> GetAccount()
        {
            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            var account = await client.GetAccountAsync();

            // Check if our account is restricted from trading.
            if (account.IsTradingBlocked)
            {
                return TypedResults.Problem("Account is currently restricted from trading.");
            }

            return TypedResults.Ok(account.BuyingPower);
        }
    }
}
