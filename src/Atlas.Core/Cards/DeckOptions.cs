namespace Atlas.Core.Cards;

/// <summary>
/// Controls the composition of a standard deck.
/// </summary>
public sealed record DeckOptions
{
    /// <summary>
    /// Gets the number of jokers to include. Valid values are 0 through 4.
    /// </summary>
    public int JokerCount { get; init; }

    internal void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(JokerCount, 0, nameof(JokerCount));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(JokerCount, 4, nameof(JokerCount));
    }
}
