namespace Atlas.Hokm;

/// <summary>
/// Represents a player's seat at the Hokm table.
/// </summary>
public enum PlayerSeat
{
    /// <summary>
    /// The north seat.
    /// </summary>
    North,

    /// <summary>
    /// The east seat.
    /// </summary>
    East,

    /// <summary>
    /// The south seat.
    /// </summary>
    South,

    /// <summary>
    /// The west seat.
    /// </summary>
    West
}

/// <summary>
/// Represents one of the two teams in a Hokm game.
/// </summary>
public enum Team
{
    /// <summary>
    /// The team consisting of the North and South seats.
    /// </summary>
    One,

    /// <summary>
    /// The team consisting of the East and West seats.
    /// </summary>
    Two
}

/// <summary>
/// Represents a player participating in a Hokm Hand.
/// </summary>
/// <param name="Id">The unique identifier of the player.</param>
/// <param name="Seat">The player's seat at the table.</param>
public sealed record HokmPlayer(
    string Id,
    PlayerSeat Seat)
{
    /// <summary>
    /// Gets the team to which the player belongs.
    /// </summary>
    public Team Team =>
        Seat is PlayerSeat.North or PlayerSeat.South
            ? Team.One
            : Team.Two;
}