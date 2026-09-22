namespace Atlas.Hokm;

/// <summary>
/// Defines how the Hakem is selected for a Hand.
/// </summary>
public enum HakemSelectionMethod
{
    /// <summary>
    /// The first player to receive an Ace becomes the Hakem.
    /// </summary>
    FirstAce,

    /// <summary>
    /// The player receiving the highest card becomes the Hakem.
    /// </summary>
    HighestCard
}