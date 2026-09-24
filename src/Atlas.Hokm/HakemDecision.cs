using Atlas.Core.Cards;

namespace Atlas.Hokm;

/// <summary>
/// Represents a Hakem's decision after the initial five-card deal.
/// </summary>
public sealed class HakemDecision
{
    private HakemDecision(
        HakemDecisionType type,
        Suit? selectedSuit = null,
        string? teammatePlayerId = null)
    {
        Type = type;
        SelectedSuit = selectedSuit;
        TeammatePlayerId = teammatePlayerId;
    }

    /// <summary>
    /// Gets the type of decision.
    /// </summary>
    public HakemDecisionType Type { get; }

    /// <summary>
    /// Gets the selected Hokm suit when the Hakem chooses a suit directly.
    /// </summary>
    public Suit? SelectedSuit { get; }

    /// <summary>
    /// Gets the teammate whose middle card is requested.
    /// </summary>
    public string? TeammatePlayerId { get; }

    /// <summary>
    /// Creates a decision where the Hakem directly chooses the Hokm suit.
    /// </summary>
    public static HakemDecision ChooseSuit(Suit suit)
    {
        return new HakemDecision(
            HakemDecisionType.ChooseSuit,
            selectedSuit: suit);
    }

    /// <summary>
    /// Creates a decision where the Hakem asks the teammate
    /// for the middle card of the teammate's original five-card hand.
    /// </summary>
    public static HakemDecision AskTeammateMiddleCard(string teammatePlayerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(teammatePlayerId);

        return new HakemDecision(
            HakemDecisionType.AskTeammateMiddleCard,
            teammatePlayerId: teammatePlayerId);
    }

    /// <summary>
    /// Creates a Naras decision.
    /// </summary>
    public static HakemDecision Naras()
    {
        return new HakemDecision(HakemDecisionType.Naras);
    }

    /// <summary>
    /// Creates a Saras decision.
    /// </summary>
    public static HakemDecision Saras()
    {
        return new HakemDecision(HakemDecisionType.Saras);
    }
}