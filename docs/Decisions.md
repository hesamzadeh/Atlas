# Architecture and Game-Rule Decisions

This document records important design decisions and the reasoning behind them.

## Card Model

Standard playing cards and Jokers are modeled as different card types.

```text
Card
├── StandardCard
│   ├── Suit
│   └── Rank
└── JokerCard
    ├── JokerColor
    └── Copy
```

A Joker is not represented as a special Suit or Rank.

This prevents invalid states such as an "Ace of Joker".

A standard card always has a valid Suit and Rank.

A Joker has neither.

## Joker Configuration

The standard deck contains 52 standard cards.

The deck may optionally contain zero through four Jokers.

Joker count is configurable through `DeckOptions`.

Hokm uses zero Jokers by default.

Future games such as Shelem can configure the deck independently.

The exact gameplay behavior of Jokers belongs to the rules of the individual game rather than to `Atlas.Core`.

## Initial Five-Card Order

The initial five cards dealt to each player preserve their original receiving order.

They are intentionally not sorted immediately.

The reason is that Hokm uses the third card of the teammate's initial five-card hand as the middle card when the Hakem requests that card to be revealed.

Therefore:

```text
First card  = hand[0]
Second card = hand[1]
Middle card = hand[2]
Fourth card = hand[3]
Fifth card  = hand[4]
```

Sorting the hand before the Hakem decision would destroy this positional information.

## Dealing Direction

Dealing direction is configurable.

The supported values are:

- Clockwise
- Counter-clockwise

The current Hakem is the starting point of the initial deal.

The player's storage order remains:

```text
North → East → South → West
```

regardless of the configured dealing direction.

## Initial Deal Timing

The deck is shuffled once at the beginning of a Hand.

The initial five cards are then dealt.

No additional shuffle occurs between the initial deal and the remaining deal.

After the Hakem establishes Hokm, the remaining cards are dealt in batches of four until all players have 13 cards.

## Hand vs Match

A Hand represents one complete Hokm deal.

A Match consists of multiple Hands.

The Match length is configurable to:

- 1 Hand
- 3 Hands
- 5 Hands
- 7 Hands

Even Match lengths are intentionally excluded because they could allow both teams to have the same number of Hand wins.

## Configuration Over Hard-Coding

Rules that may legitimately vary between game modes or lobby configurations should be represented as configuration rather than embedded as constants in the game engine.

Current configurable Hokm settings include:

- Match length
- Dealing direction
- Hakem selection method
- Hakem rotation direction
- Naras availability
- Saras availability
- Joker count

This allows the platform to support different lobby configurations without duplicating the game engine.

## Separation of Rules and Infrastructure

The Hokm rules should not depend on:

- Flutter
- SignalR
- PostgreSQL
- User accounts
- Authentication
- Networking
- UI

The game engine should be capable of running entirely in memory.

This makes it possible to test and simulate the game independently from the eventual multiplayer infrastructure.

## Command and Event Direction

The intended multiplayer architecture follows a command/event mindset.

For example:

```text
PlayCardCommand
        ↓
Game engine validates the move
        ↓
CardPlayedEvent
        ↓
Game state updated
        ↓
State/event broadcast
```

The exact event infrastructure will be implemented later.

The important decision is that the server-side game engine remains the authority over legal game actions.

## Illegal States

Whenever practical, the architecture should make illegal states unrepresentable.

Examples:

- A `StandardCard` cannot exist without a valid Suit and Rank.
- A `JokerCard` does not have a Suit or Rank.
- A Hokm Hand requires exactly four players.
- Player seats must be unique.
- Player IDs must be unique.
- The Hakem must be one of the players.
- A Match length must be one of the supported odd values.
- Joker count must remain within the supported range.