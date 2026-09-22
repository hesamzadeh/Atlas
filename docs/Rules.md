# Hokm Rules

## Match Structure

A Match consists of multiple Hands.

A Hand is one complete Hokm deal and consists of up to 13 Tricks.

The number of Hands required to win a Match is configurable:

- 1 Hand
- 3 Hands
- 5 Hands
- 7 Hands

Only odd values are supported so that a Match cannot end with both teams having the same number of Hand wins.

The classic configuration is 7 Hands.

## Players and Teams

A Hokm Hand has exactly four players.

Players occupy four seats:

- North
- East
- South
- West

Opposite seats are teammates:

- North + South = Team One
- East + West = Team Two

## Initial Deal

The deck is shuffled once at the beginning of the Hand.

Each player initially receives five cards.

The initial five cards are dealt one card at a time around the table according to the configured dealing direction.

The dealing direction may be:

- Clockwise
- Counter-clockwise

The deal begins with the current Hakem.

## Initial Card Order

The order in which the five initial cards are received must be preserved.

The initial five cards must not be sorted before the Hakem decision is complete.

The position of each card is significant because the Hakem may request the middle card of the teammate's initial hand.

The middle card is explicitly defined as the third card in the original deal order:

`hand[2]`

Therefore:

- First card = `hand[0]`
- Second card = `hand[1]`
- Middle card = `hand[2]`
- Fourth card = `hand[3]`
- Fifth card = `hand[4]`

## Hakem Decision

After all four players have received their initial five cards, the Hakem makes the Hokm decision.

The available decisions are:

- Choose a suit as Hokm directly.
- Request the middle card of the teammate's initial five-card hand to be revealed. The suit of that card becomes Hokm.
- Call Naras, when enabled by the Match configuration.
- Call Saras, when enabled by the Match configuration.

The detailed decision-state rules will be implemented by the game engine.

## Remaining Deal

After Hokm has been established, the remaining cards are dealt in batches of four cards to each player.

The four-card batches continue until every player has 13 cards.

Each player therefore receives:

- 5 cards during the initial deal
- 8 additional cards after Hokm is established
- 13 cards total

Once all 13 cards have been dealt, the cards may be sorted for normal gameplay and display.

The original initial-five-card order must have been preserved until the Hakem decision has been resolved.

## First Trick

After all players have received their 13 cards, the Hakem starts the first Trick.

The remaining trick rules will be implemented by the game engine.

## Hakem Selection

The method used to determine the Hakem is configurable.

Supported selection methods include:

- First Ace
- Highest Card

If a selection method requires a tie to be resolved, the tie-resolution rule is part of the Hakem-selection logic.