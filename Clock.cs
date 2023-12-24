#pragma warning disable CS1591
public class Clock
{
    //
    // Summary:
    //     Gets current timestamp in UTC.
    DateTime TimestampUtc { get; }

    //
    // Summary:
    //     Returns true if trading day is open now.
    bool IsOpen { get; }

    //
    // Summary:
    //     Gets nearest trading day open time in UTC.
    DateTime NextOpenUtc { get; }

    //
    // Summary:
    //     Gets nearest trading day close time in UTC.
    DateTime NextCloseUtc { get; }
}
#pragma warning restore CS1591
