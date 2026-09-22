# Architecture

## Project Structure

Atlas separates the card-game rules from networking, persistence, UI, and platform concerns.

The current solution is structured as:

```text
Atlas/
├── src/
│   ├── Atlas.Core/
│   ├── Atlas.Hokm/
│   └── Atlas.Console/
└── tests/
    └── Atlas.Hokm.Tests/
```

The dependency direction is:

```text
Atlas.Core
    ↑
Atlas.Hokm
    ↑
Atlas.Console

Atlas.Hokm.Tests
    ↑
Atlas.Hokm
    ↑
Atlas.Core
```

The game engine must not depend on the console application.

## Atlas.Core

`Atlas.Core` contains generic card-game primitives.

Examples include:

- `Card`
- `StandardCard`
- `JokerCard`
- `Suit`
- `Rank`
- `Deck`
- `DeckOptions`

The Core layer does not know anything about:

- Hokm
- Players
- Teams
- Networking
- UI
- Databases
- Accounts

This keeps the generic card model reusable by future games.

## Atlas.Hokm

`Atlas.Hokm` contains Hokm-specific rules and game state.

Examples include:

- Players
- Seats
- Teams
- Hands
- Hakem
- Hokm configuration
- Dealing rules
- Future trick rules
- Future scoring rules

The Hokm engine should be usable without knowing whether the game is being played through:

- Console
- Flutter
- SignalR
- A database
- Another future client

## Atlas.Console

`Atlas.Console` is a development and simulation environment.

It will eventually be used to:

- Start simulated matches
- Run bot players
- Print game state
- Debug rules
- Reproduce game scenarios

The Hokm engine does not depend on the console application.

## Atlas.Hokm.Tests

`Atlas.Hokm.Tests` contains automated tests for Hokm behavior and the underlying card-game functionality.

Game rules should be covered by tests before higher-level systems are built around them.

---

## Game State and UI State

The game engine owns the authoritative game state.

The UI should not determine whether an action is legal.

For example:

```text
Player requests action
        ↓
Game engine validates action
        ↓
Game state changes
        ↓
Event/state update is produced
        ↓
UI displays the result
```

This allows the same game rules to be used by:

- Console simulation
- Bots
- Flutter clients
- Server-side multiplayer
- Future spectators

---

## Initial Deal State

The initial five-card deal preserves the exact order in which cards were received.

This is intentional.

The Hakem decision depends on the original position of the teammate's third card.

Therefore, sorting the hand immediately after dealing would destroy meaningful game state.

The engine currently preserves the deal order directly in the player's hand.

Once the Hakem decision is complete and all 13 cards have been dealt, the cards may be sorted for gameplay and display.

The gameplay/display ordering must not change the underlying rules or destroy information required by the game engine.

---

## Dealing Direction

Dealing direction is modeled independently from the player's storage order.

Players are stored in table order:

```text
North → East → South → West
```

The configured dealing direction determines how the engine traverses those seats.

For example, when North is Hakem:

### Clockwise

```text
North → East → South → West
```

### Counter-clockwise

```text
North → West → South → East
```

This keeps the physical table position separate from the direction in which cards are dealt.

---

## Hand and Match Separation

The engine distinguishes between:

- Match
- Hand
- Trick

A Match contains multiple Hands.

A Hand contains up to 13 Tricks.

A Trick contains one played card from each of the four players.

This separation allows match-level scoring and rotation rules to remain independent from individual trick logic.

---

## Testing Strategy

Important game rules should be represented by automated tests before higher-level game behavior is implemented.

The initial-deal tests verify:

- Exactly five cards per player
- Correct dealing direction
- Complete per-player deal order
- Preservation of original card order
- Correct middle-card position
- Remaining deck size

Future tests will cover:

- Hakem selection
- Hokm selection
- Follow-suit rules
- Trump rules
- Trick winners
- Hand scoring
- Match scoring
- Hakem rotation
- Bot behavior
