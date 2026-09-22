namespace Atlas.Hokm;

/// <summary>
/// Defines the configurable rules and settings for a Hokm match.
/// </summary>
public sealed class HokmOptions
{
    /// <summary>
    /// Gets the number of Hands a team must win to win the Match.
    /// </summary>
    public int HandsToWin { get; init; } = 7;

    /// <summary>
    /// Gets the direction in which cards are dealt.
    /// </summary>
    public DealingDirection DealingDirection { get; init; } =
        DealingDirection.Clockwise;

    /// <summary>
    /// Gets the method used to determine the Hakem.
    /// </summary>
    public HakemSelectionMethod HakemSelectionMethod { get; init; } =
        HakemSelectionMethod.FirstAce;

    /// <summary>
    /// Gets the direction in which the Hakem position rotates between Hands.
    /// </summary>
    public HakemRotationDirection HakemRotationDirection { get; init; } =
        HakemRotationDirection.Clockwise;

    /// <summary>
    /// Gets a value indicating whether Naras is allowed in the Match.
    /// </summary>
    public bool AllowNaras { get; init; }

    /// <summary>
    /// Gets a value indicating whether Saras is allowed in the Match.
    /// </summary>
    public bool AllowSaras { get; init; }

    /// <summary>
    /// Gets the number of Jokers included in the deck.
    /// Normal Hokm uses zero Jokers.
    /// </summary>
    public int JokerCount { get; init; }

    /// <summary>
    /// Validates the configured Hokm rules.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when a setting contains an unsupported value.
    /// </exception>
    public void Validate()
    {
        if (HandsToWin is not (1 or 3 or 5 or 7))
        {
            throw new ArgumentOutOfRangeException(
                nameof(HandsToWin),
                "Hands to win must be 1, 3, 5, or 7.");
        }

        if (JokerCount is < 0 or > 4)
        {
            throw new ArgumentOutOfRangeException(
                nameof(JokerCount),
                "Joker count must be between 0 and 4.");
        }
    }
}