namespace Atlas.Core.Cards;

/// <summary>
/// Represents a joker, which has no suit or rank.
/// </summary>
public sealed record JokerCard : Card
{
    /// <summary>
    /// Initializes a joker. A deck can contain up to two copies of each color.
    /// </summary>
    public JokerCard(JokerColor color, int copy = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(true, Enum.IsDefined(color), nameof(color));
        ArgumentOutOfRangeException.ThrowIfLessThan(copy, 1, nameof(copy));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(copy, 2, nameof(copy));

        Color = color;
        Copy = copy;
    }

    public JokerColor Color { get; }

    public int Copy { get; }

    public override string ToString() => Copy == 1 ? $"{Color} Joker" : $"{Color} Joker ({Copy})";
}
