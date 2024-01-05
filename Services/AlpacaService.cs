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
                return TypedResults.Problem($"Failed to convert string '{exchange}' to Exchange type enum value.");
            }

            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            var listAssets = await client.ListAssetsAsync(new AssetsRequest { AssetStatus = AssetStatus.Active, Exchange = exchangeEnum });

            // Check if any active assets are available
            if (!listAssets.Any())
            {
                return TypedResults.Problem("No active assets available for trading.");
            }

            return TypedResults.Ok(listAssets.Select(assets => assets.Name + " : " + assets.Symbol).OrderBy(name => name));
        }

        /// <summary>
        /// Submit Market Buy Order
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> SubmitMarketBuyOrder(TradingViewMessage message)
        {
            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            if (string.IsNullOrWhiteSpace(message.Ticker) || string.IsNullOrWhiteSpace(message.MarketPositionSize))
            {
                return TypedResults.Problem("Ticker or Market Position Size is empty");
            }

            bool result = int.TryParse(message.MarketPositionSize, out int marketPositionSize);

            if (!result)
            {
                return TypedResults.Problem("MarketPositionSize could not be parse to int");
            }

            if (marketPositionSize <= 0)
            {
                return TypedResults.Problem("MarketPositionSize is zero or negative");
            }

            try
            {
                var asset = await client.GetAssetAsync(message.Ticker);

                if (asset.IsTradable)
                {
                    // Submit a market order to buy 1 share of given symbol at market price
                    IOrder submittedOrder = await client.PostOrderAsync(MarketOrder.Buy(message.Ticker, Convert.ToInt32(message.MarketPositionSize)));
                }
                else
                {
                    return TypedResults.Problem($"{message.Ticker} found but is not tradeable");
                }
            }
            catch (Exception exception)
            {
                return TypedResults.Problem(exception.Message);
            }
            
            return TypedResults.Ok($"Market Order Buy executed for symbol {message.Ticker}");
        }

        /// <summary>
        /// Cancel Order
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> CancelOrder(string orderId)
        {
            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            var guid = new Guid(orderId);

            IOrder order;

            try
            {
                order = await client.GetOrderAsync(guid);
            }
            catch (Exception exception)
            {
                return TypedResults.Problem(exception.Message);
            }

            // Cancel order by supplying orderid guid
            var canceledOrder = await client.CancelOrderAsync(order.OrderId);

            return TypedResults.Ok($"Order with OrderId {guid} cancelled");
        }

        /// <summary>
        /// Submit Market Sell Order
        /// </summary>
        /// <returns></returns>
        public static async Task<IResult> SubmitMarketSellOrder(string symbol)
        {
            var client = Alpaca.Markets.Environments.Paper
                      .GetAlpacaTradingClient(new SecretKey(ApiConstants.AlpacaKeyId, ApiConstants.AlpacaSecretKey));

            try
            {
                // Submit a market order to sell 1 share of given symbol at market price
                var order = await client.PostOrderAsync(MarketOrder.Sell(symbol, 1));
            }
            catch (Exception exception)
            {
                return TypedResults.Problem(exception.Message);
            }

            return TypedResults.Ok($"Market Order Sell executed for symbol {symbol}");
        }

        #region AlpacaService helpers

        // Custom method to get enum value from EnumMemberAttribute value
        private static T? GetEnumValueFromEnumMemberAttribute<T>(string value)
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
                        return (T?)field.GetValue(obj: null);
                    }
                }
            }

            return default; // Return default enum value if not found
        }

        #endregion
    }
}
