using Atlas.Core.Cards;
using Atlas.Hokm;

namespace Atlas.Hokm.Tests;

/// <summary>
/// Tests the initialization and initial dealing rules of a Hokm Hand.
/// </summary>
public sealed class HokmHandTests
{
    [Fact]
    public void HokmHand_RequiresExactlyFourPlayers()
    {
        var players = new[]
        {
            new HokmPlayer("north", PlayerSeat.North),
            new HokmPlayer("east", PlayerSeat.East),
            new HokmPlayer("south", PlayerSeat.South)
        };

        Assert.Throws<ArgumentException>(
            () => new HokmHand(players, "north"));
    }

    [Fact]
    public void HokmHand_AssignsOppositeSeatsToTheSameTeam()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");

        Assert.Equal(Team.One, hand.Players[0].Team);
        Assert.Equal(Team.Two, hand.Players[1].Team);
        Assert.Equal(Team.One, hand.Players[2].Team);
        Assert.Equal(Team.Two, hand.Players[3].Team);
    }

    [Fact]
    public void HokmHand_IdentifiesHakemAndTeammate()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");

        Assert.Equal("north", hand.Hakem.Id);

        var teammate = hand.GetTeammate("north");

        Assert.Equal("south", teammate.Id);
    }

    [Fact]
    public void InitialDeal_GivesFiveCardsToEveryPlayer()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");
        var deck = Deck.CreateStandard();

        deck.Shuffle(new Random(42));

        var dealer = new HokmHandDealer();

        dealer.DealInitialCards(
            hand,
            deck,
            DealingDirection.Clockwise);

        foreach (var player in players)
        {
            Assert.Equal(5, hand.GetHand(player.Id).Count);
        }

        Assert.Equal(32, deck.Count);
    }

    [Fact]
    public void InitialDeal_PreservesCompleteDealOrderForEveryPlayer()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");
        var deck = Deck.CreateStandard();

        var firstTwenty = deck.Cards.Take(20).ToArray();

        var dealer = new HokmHandDealer();

        dealer.DealInitialCards(
            hand,
            deck,
            DealingDirection.Clockwise);

        var dealingOrder = new[]
        {
            "north",
            "east",
            "south",
            "west"
        };

        for (var playerIndex = 0; playerIndex < dealingOrder.Length; playerIndex++)
        {
            var playerId = dealingOrder[playerIndex];
            var actualHand = hand.GetHand(playerId);

            var expectedHand = Enumerable
                .Range(0, 5)
                .Select(round => firstTwenty[(round * 4) + playerIndex])
                .ToArray();

            Assert.Equal(expectedHand, actualHand);
        }
    }

    [Fact]
    public void InitialDeal_MiddleCardIsTheThirdCardReceived()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");
        var deck = Deck.CreateStandard();

        deck.Shuffle(new Random(42));

        var dealer = new HokmHandDealer();

        dealer.DealInitialCards(
            hand,
            deck,
            DealingDirection.Clockwise);

        var southHand = hand.GetHand("south");

        Assert.Equal(southHand[2], hand.GetMiddleCard("south"));
    }

    [Fact]
    public void InitialDeal_ClockwiseStartsWithHakemAndMovesClockwise()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");
        var deck = Deck.CreateStandard();

        var firstTwenty = deck.Cards.Take(20).ToArray();

        var dealer = new HokmHandDealer();

        dealer.DealInitialCards(
            hand,
            deck,
            DealingDirection.Clockwise);

        Assert.Equal(firstTwenty[0], hand.GetHand("north")[0]);
        Assert.Equal(firstTwenty[1], hand.GetHand("east")[0]);
        Assert.Equal(firstTwenty[2], hand.GetHand("south")[0]);
        Assert.Equal(firstTwenty[3], hand.GetHand("west")[0]);
    }

    [Fact]
    public void InitialDeal_CounterClockwiseStartsWithHakemAndMovesCounterClockwise()
    {
        var players = CreatePlayers();
        var hand = new HokmHand(players, "north");
        var deck = Deck.CreateStandard();

        var firstTwenty = deck.Cards.Take(20).ToArray();

        var dealer = new HokmHandDealer();

        dealer.DealInitialCards(
            hand,
            deck,
            DealingDirection.CounterClockwise);

        Assert.Equal(firstTwenty[0], hand.GetHand("north")[0]);
        Assert.Equal(firstTwenty[1], hand.GetHand("west")[0]);
        Assert.Equal(firstTwenty[2], hand.GetHand("south")[0]);
        Assert.Equal(firstTwenty[3], hand.GetHand("east")[0]);
    }

    private static IReadOnlyList<HokmPlayer> CreatePlayers()
    {
        return
        [
            new HokmPlayer("north", PlayerSeat.North),
            new HokmPlayer("east", PlayerSeat.East),
            new HokmPlayer("south", PlayerSeat.South),
            new HokmPlayer("west", PlayerSeat.West)
        ];
    }
}