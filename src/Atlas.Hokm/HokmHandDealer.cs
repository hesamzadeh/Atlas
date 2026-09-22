using Atlas.Core.Cards;

namespace Atlas.Hokm;

/// <summary>
/// Deals the initial cards for a Hokm Hand.
/// </summary>
public sealed class HokmHandDealer
{
    /// <summary>
    /// Deals the initial five cards to each player.
    /// </summary>
    /// <param name="hand">The Hokm Hand being initialized.</param>
    /// <param name="deck">The shuffled deck to draw cards from.</param>
    /// <param name="direction">The direction in which cards are dealt.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied hand is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the deck does not contain enough cards.
    /// </exception>
    public void DealInitialCards(
        HokmHand hand,
        Deck deck,
        DealingDirection direction)
    {
        ArgumentNullException.ThrowIfNull(hand);
        ArgumentNullException.ThrowIfNull(deck);

        if (deck.Count < 20)
        {
            throw new InvalidOperationException(
                "The deck must contain at least 20 cards to deal the initial five cards.");
        }

        var players = GetDealingOrder(hand, direction);

        for (var cardIndex = 0; cardIndex < 5; cardIndex++)
        {
            foreach (var player in players)
            {
                hand.AddCard(player.Id, deck.Draw());
            }
        }
    }

    /// <summary>
    /// Determines the order in which players receive cards.
    /// </summary>
    /// <param name="hand">The Hokm Hand.</param>
    /// <param name="direction">The dealing direction.</param>
    /// <returns>The players in dealing order.</returns>
    private static IReadOnlyList<HokmPlayer> GetDealingOrder(
        HokmHand hand,
        DealingDirection direction)
    {
        var hakemIndex = hand.Players
            .Select((player, index) => new { player, index })
            .Single(item => item.player.Id == hand.HakemId)
            .index;

        var players = hand.Players;

        var orderedPlayers = new List<HokmPlayer>(4);

        var step = direction == DealingDirection.Clockwise ? 1 : -1;

        for (var offset = 0; offset < players.Count; offset++)
        {
            var index = (hakemIndex + (offset * step)) % players.Count;

            if (index < 0)
            {
                index += players.Count;
            }

            orderedPlayers.Add(players[index]);
        }

        return orderedPlayers;
    }
}