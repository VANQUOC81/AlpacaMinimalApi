#pragma warning disable CS1591
public class TradingViewMessage
{
    // "time": "{{timenow}}"
    public string? Time { get; set; }

    // "exchange": "{{exchange}}"
    public string? Exchange { get; set; }

    // "ticker": "{{ticker}}",
    public string? Ticker { get; set; }

    // "position_size": "{{strategy.position_size}}"
    public string? PositionSize { get; set; }

    // "order_action": "{{strategy.order.action}}"
    public string? OrderAction { get; set; }

    // "order_contracts": "{{strategy.order.contracts}}"
    public string? OrderContracts { get; set; }

    // "order_price": "{{strategy.order.price}}"
    public string? OrderPrice { get; set; }

    // "order_id": "{{strategy.order.id}}"
    public string? OrderId { get; set; }

    // "market_position": "{{strategy.market_position}}"
    public string? MarketPosition { get; set; }

    // "market_position_size": "{{strategy.market_position_size}}"
    public string? MarketPositionSize { get; set; }

    // "prev_market_position": "{{strategy.prev_market_position}}"
    public string? PrevMarketPosition { get; set; }

    // "prev_market_position_size": "{{strategy.prev_market_position_size}}"
    public string? PrevMarketPositionSize { get; set; }
}
#pragma warning restore CS1591
