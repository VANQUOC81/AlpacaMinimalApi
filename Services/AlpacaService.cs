using System.Reflection;
using System.Runtime.Serialization;
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

            return TypedResults.Ok(account);
        }

        /// <summary>
        /// Get all active assets from exchange.
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> ListAssets(string exchange)
        {
            // Convert the string to the Exchange enum type
            Exchange exchangeEnum = GetEnumValueFromEnumMemberAttribute<Exchange>(exchange);
            
            if (exchangeEnum == Exchange.Unknown)
            {
                // String doesn't match any enum value
                return TypedResults.Problem($"Failed to convert string '{exchange}' to enum value.");
            }

            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            var listAssets = await client.ListAssetsAsync(new AssetsRequest { AssetStatus = AssetStatus.Active, Exchange = exchangeEnum });

            // Check if any active assets are available
            if (!listAssets.Any())
            {
                return TypedResults.Problem("No active assets available for trading.");
            }

            return TypedResults.Ok(listAssets.Count);
        }

    // Custom method to get enum value from EnumMemberAttribute value
    private static T GetEnumValueFromEnumMemberAttribute<T>(string value)
    {
        Type enumType = typeof(T);

        // Check if the type is an enum
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        // Iterate through each enum member and get the EnumMemberAttribute value
        foreach (FieldInfo field in enumType.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
            {
                if (attribute.Value == value)
                {
                    return (T)field.GetValue(null);
                }
            }
        }

        return default; // Return default enum value if not found
    }
    }
}
