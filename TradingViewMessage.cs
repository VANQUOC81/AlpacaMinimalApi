#pragma warning disable CS1591
public class TradingViewMessage
{
    public string? Symbol { get; set; }
    
    public int Quantity { get; set; }

    public string? TimeNow { get; set; }
    //   "time": "{{timenow}}",
    //   "exchange": "{{exchange}}",
    //   "ticker": "{{ticker}}",
    //   "position_size": "{{strategy.position_size}}",
    //   "order_action": "{{strategy.order.action}}",
    //   "order_contracts": "{{strategy.order.contracts}}",
    //   "order_price": "{{strategy.order.price}}",
    //   "order_id": "{{strategy.order.id}}",
    //   "market_position": "{{strategy.market_position}}",
    //   "market_position_size": "{{strategy.market_position_size}}",
    //   "prev_market_position": "{{strategy.prev_market_position}}",
    //   "prev_market_position_size": "{{strategy.prev_market_position_size}}"
}
#pragma warning restore CS1591
