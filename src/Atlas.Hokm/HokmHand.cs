using Atlas.Core.Cards;

namespace Atlas.Hokm;

/// <summary>
/// Represents the state of a single Hokm Hand.
/// </summary>
public sealed class HokmHand
{
    private readonly Dictionary<string, List<Card>> _hands;
    private HakemDecision? _hakemDecision;
    private Suit? _hokmSuit;
    private readonly HokmOptions _options;
    
    /// <summary>
    /// Gets the four players participating in the Hand.
    /// </summary>
    public IReadOnlyList<HokmPlayer> Players { get; }

    /// <summary>
    /// Gets the decision made by the Hakem.
    /// </summary>
    public HakemDecision? HakemDecision => _hakemDecision;

    /// <summary>
    /// Gets the established Hokm suit.
    /// </summary>
    public Suit? HokmSuit => _hokmSuit;
    
    /// <summary>
    /// Gets the identifier of the player who is currently the Hakem.
    /// </summary>
    public string HakemId { get; }

    /// <summary>
    /// Gets the cards currently held by each player.
    /// Card order is preserved exactly as cards were dealt.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<Card>> Hands =>
        _hands.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<Card>)pair.Value.AsReadOnly());

    /// <summary>
    /// Initializes a new Hokm Hand.
    /// </summary>
    /// <param name="players">The four players participating in the Hand.</param>
    /// <param name="hakemId">The identifier of the player who is the Hakem.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the players or Hakem are invalid.
    /// </exception>
    public HokmHand(
        IReadOnlyList<HokmPlayer> players,
        string hakemId,
        HokmOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentException.ThrowIfNullOrWhiteSpace(hakemId);

        if (players.Count != 4)
        {
            throw new ArgumentException(
                "A Hokm Hand requires exactly four players.",
                nameof(players));
        }

        if (players.Select(p => p.Id).Distinct().Count() != 4)
        {
            throw new ArgumentException(
                "Player IDs must be unique.",
                nameof(players));
        }

        if (!players.Any(p => p.Id == hakemId))
        {
            throw new ArgumentException(
                "The Hakem must be one of the four players.",
                nameof(hakemId));
        }

        if (players.Select(p => p.Seat).Distinct().Count() != 4)
        {
            throw new ArgumentException(
                "Each player must have a unique seat.",
                nameof(players));
        }

        _options = options ?? new HokmOptions();
        
        Players = players;
        HakemId = hakemId;

        _hands = players.ToDictionary(
            player => player.Id,
            _ => new List<Card>());
    }

    /// <summary>
    /// Applies the Hakem's decision and establishes the Hokm suit
    /// when the decision determines it.
    /// </summary>
    public void ApplyHakemDecision(HakemDecision decision)
    {
        ArgumentNullException.ThrowIfNull(decision);

        if (_hakemDecision is not null)
        {
            throw new InvalidOperationException(
                "The Hakem has already made a decision.");
        }

        switch (decision.Type)
        {
            case HakemDecisionType.ChooseSuit:
                if (decision.SelectedSuit is null)
                {
                    throw new InvalidOperationException(
                        "A ChooseSuit decision must contain a suit.");
                }

                _hokmSuit = decision.SelectedSuit;
                break;

            case HakemDecisionType.AskTeammateMiddleCard:
                if (decision.TeammatePlayerId is null)
                {
                    throw new InvalidOperationException(
                        "An AskTeammateMiddleCard decision must contain a teammate.");
                }

                var teammate = GetTeammate(HakemId);

                if (decision.TeammatePlayerId != teammate.Id)
                {
                    throw new InvalidOperationException(
                        "The requested player must be the Hakem's teammate.");
                }

                var middleCard = GetMiddleCard(teammate.Id);

                if (middleCard is not StandardCard standardCard)
                {
                    throw new InvalidOperationException(
                        "The teammate's middle card must be a standard card.");
                }

                _hokmSuit = standardCard.Suit;
                break;

            case HakemDecisionType.Naras:
                if (!_options.AllowNaras)
                {
                    throw new InvalidOperationException(
                        "Naras is not allowed for this Hand.");
                }

                break;

            case HakemDecisionType.Saras:
                if (!_options.AllowSaras)
                {
                    throw new InvalidOperationException(
                        "Saras is not allowed for this Hand.");
                }

                break; 

            default:
                throw new ArgumentOutOfRangeException();
        }

        _hakemDecision = decision;
    } 
    
    /// <summary>
    /// Gets the current cards held by a player.
    /// </summary>
    /// <param name="playerId">The player's identifier.</param>
    /// <returns>The player's cards in their original deal order.</returns>
    public IReadOnlyList<Card> GetHand(string playerId)
    {
        if (!_hands.TryGetValue(playerId, out var hand))
        {
            throw new ArgumentException(
                $"Unknown player: {playerId}",
                nameof(playerId));
        }

        return hand.AsReadOnly();
    }

    /// <summary>
    /// Adds a card to a player's hand while preserving deal order.
    /// </summary>
    /// <param name="playerId">The player's identifier.</param>
    /// <param name="card">The card to add.</param>
    internal void AddCard(string playerId, Card card)
    {
        ArgumentNullException.ThrowIfNull(card);

        if (!_hands.TryGetValue(playerId, out var hand))
        {
            throw new ArgumentException(
                $"Unknown player: {playerId}",
                nameof(playerId));
        }

        hand.Add(card);
    }

    /// <summary>
    /// Gets the current Hakem.
    /// </summary>
    public HokmPlayer Hakem =>
        Players.Single(player => player.Id == HakemId);

    /// <summary>
    /// Gets the teammate of the specified player.
    /// </summary>
    /// <param name="playerId">The player's identifier.</param>
    /// <returns>The player's teammate.</returns>
    public HokmPlayer GetTeammate(string playerId)
    {
        var player = Players.SingleOrDefault(p => p.Id == playerId);

        if (player is null)
        {
            throw new ArgumentException(
                $"Unknown player: {playerId}",
                nameof(playerId));
        }

        return Players.Single(
            p => p.Team == player.Team && p.Id != player.Id);
    }

    /// <summary>
    /// Gets the middle card from a player's initial five-card deal.
    /// The middle card is the third card in the original deal order.
    /// </summary>
    /// <param name="playerId">The player's identifier.</param>
    /// <returns>The third card in the player's initial deal.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the player has fewer than three cards.
    /// </exception>
    public Card GetMiddleCard(string playerId)
    {
        var hand = GetHand(playerId);

        if (hand.Count < 3)
        {
            throw new InvalidOperationException(
                "The player must have at least three cards to have a middle card.");
        }

        return hand[2];
    }
}