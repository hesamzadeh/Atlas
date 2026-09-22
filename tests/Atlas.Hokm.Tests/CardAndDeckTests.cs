using Atlas.Core.Cards;

namespace Atlas.Hokm.Tests;

public sealed class CardAndDeckTests
{
    [Fact]
    public void StandardCard_StoresItsSuitAndRank()
    {
        var card = new StandardCard(Suit.Hearts, Rank.Ace);

        Assert.Equal(Suit.Hearts, card.Suit);
        Assert.Equal(Rank.Ace, card.Rank);
    }

    [Fact]
    public void StandardCard_RejectsUndefinedSuitOrRank()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StandardCard((Suit)99, Rank.Ace));
        Assert.Throws<ArgumentOutOfRangeException>(() => new StandardCard(Suit.Clubs, (Rank)99));
    }

    [Fact]
    public void JokerCard_HasNoSuitOrRank_AndIdentifiesItsCopy()
    {
        var joker = new JokerCard(JokerColor.Black, 2);

        Assert.Equal(JokerColor.Black, joker.Color);
        Assert.Equal(2, joker.Copy);
        Assert.DoesNotContain(typeof(JokerCard).GetProperties(), property => property.Name is "Suit" or "Rank");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    public void StandardDeck_RejectsUnsupportedJokerCounts(int jokerCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Deck.CreateStandard(new DeckOptions { JokerCount = jokerCount }));
    }

    [Theory]
    [InlineData(0, 52)]
    [InlineData(2, 54)]
    [InlineData(4, 56)]
    public void StandardDeck_ContainsEveryStandardCardAndRequestedJokers(int jokerCount, int expectedCount)
    {
        var deck = Deck.CreateStandard(new DeckOptions { JokerCount = jokerCount });

        Assert.Equal(expectedCount, deck.Count);
        Assert.Equal(expectedCount, deck.Cards.Distinct().Count());
        Assert.Equal(52, deck.Cards.OfType<StandardCard>().Count());
        Assert.Equal(jokerCount, deck.Cards.OfType<JokerCard>().Count());
    }

    [Fact]
    public void DrawMany_IsAtomicWhenDeckDoesNotHaveEnoughCards()
    {
        var deck = new Deck([new StandardCard(Suit.Spades, Rank.Ace)]);

        Assert.Throws<InvalidOperationException>(() => deck.Draw(2));
        Assert.Equal(1, deck.Count);
    }

    [Fact]
    public void Shuffle_WithSeededRandom_PreservesEveryCard()
    {
        var deck = Deck.CreateStandard();
        var original = deck.Cards.OrderBy(card => card.ToString()).ToArray();

        deck.Shuffle(new Random(42));

        Assert.Equal(original, deck.Cards.OrderBy(card => card.ToString()));
        Assert.NotEqual(Deck.CreateStandard().Cards, deck.Cards);
    }
}
