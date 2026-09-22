namespace Atlas.Core.Cards;

/// <summary>
/// A mutable collection of cards that supports shuffling and drawing.
/// </summary>
public sealed class Deck
{
    private readonly List<Card> _cards;

    /// <summary>
    /// Initializes a deck from distinct cards in the supplied order.
    /// </summary>
    public Deck(IEnumerable<Card> cards)
    {
        ArgumentNullException.ThrowIfNull(cards);

        _cards = cards.ToList();
        if (_cards.Any(card => card is null))
        {
            throw new ArgumentException("A deck cannot contain null cards.", nameof(cards));
        }

        if (_cards.Count != _cards.Distinct().Count())
        {
            throw new ArgumentException("A deck cannot contain duplicate cards.", nameof(cards));
        }
    }

    /// <summary>
    /// Gets the cards remaining in their current draw order.
    /// </summary>
    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    /// <summary>
    /// Gets the number of cards remaining.
    /// </summary>
    public int Count => _cards.Count;

    /// <summary>
    /// Creates an ordered 52-card deck, with an optional zero to four jokers.
    /// </summary>
    public static Deck CreateStandard(DeckOptions? options = null)
    {
        options ??= new DeckOptions();
        options.Validate();

        var cards = new List<Card>(52 + options.JokerCount);
        foreach (var suit in Enum.GetValues<Suit>())
        {
            foreach (var rank in Enum.GetValues<Rank>())
            {
                cards.Add(new StandardCard(suit, rank));
            }
        }

        var jokers = new[]
        {
            new JokerCard(JokerColor.Red),
            new JokerCard(JokerColor.Black),
            new JokerCard(JokerColor.Red, 2),
            new JokerCard(JokerColor.Black, 2)
        };
        cards.AddRange(jokers.Take(options.JokerCount));

        return new Deck(cards);
    }

    /// <summary>
    /// Randomly rearranges the remaining cards using Fisher–Yates shuffling.
    /// </summary>
    public void Shuffle(Random? random = null)
    {
        random ??= Random.Shared;
        for (var index = _cards.Count - 1; index > 0; index--)
        {
            var swapIndex = random.Next(index + 1);
            (_cards[index], _cards[swapIndex]) = (_cards[swapIndex], _cards[index]);
        }
    }

    /// <summary>
    /// Draws the next card.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the deck is empty.</exception>
    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("Cannot draw from an empty deck.");
        }

        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }

    /// <summary>
    /// Draws the requested number of cards atomically.
    /// </summary>
    public IReadOnlyList<Card> Draw(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        if (count > _cards.Count)
        {
            throw new InvalidOperationException("The deck does not contain enough cards.");
        }

        var drawn = _cards.Take(count).ToArray();
        _cards.RemoveRange(0, count);
        return drawn;
    }
}
