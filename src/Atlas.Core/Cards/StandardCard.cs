namespace Atlas.Core.Cards;

/// <summary>
/// Represents one of the 52 cards that has a suit and rank.
/// </summary>
public sealed record StandardCard : Card
{
    /// <summary>
    /// Initializes a standard playing card.
    /// </summary>
    public StandardCard(Suit suit, Rank rank)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(true, Enum.IsDefined(suit), nameof(suit));
        ArgumentOutOfRangeException.ThrowIfNotEqual(true, Enum.IsDefined(rank), nameof(rank));

        Suit = suit;
        Rank = rank;
    }

    public Suit Suit { get; }

    public Rank Rank { get; }

    public override string ToString() => $"{Rank} of {Suit}";
}
