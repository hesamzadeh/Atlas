namespace Atlas.Hokm;

/// <summary>
/// Represents the type of decision made by the Hakem
/// after receiving the initial five cards.
/// </summary>
public enum HakemDecisionType
{
    /// <summary>
    /// The Hakem directly chooses the Hokm suit.
    /// </summary>
    ChooseSuit,

    /// <summary>
    /// The Hakem asks the teammate for the middle card
    /// of the teammate's original five-card hand.
    /// </summary>
    AskTeammateMiddleCard,

    /// <summary>
    /// The Hakem declares Naras.
    /// </summary>
    Naras,

    /// <summary>
    /// The Hakem declares Saras.
    /// </summary>
    Saras
}