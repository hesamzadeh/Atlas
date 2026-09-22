# Atlas Roadmap

## Phase 1 — Game Engine Foundation

### Completed

- [x] Create solution and project structure
- [x] Create `Atlas.Core`
- [x] Create `Atlas.Hokm`
- [x] Create `Atlas.Console`
- [x] Create `Atlas.Hokm.Tests`
- [x] Implement standard card model
- [x] Implement Joker card model
- [x] Implement configurable deck with 0–4 Jokers
- [x] Implement Hokm player and team model
- [x] Implement Hokm Hand
- [x] Implement configurable dealing direction
- [x] Implement initial five-card deal
- [x] Preserve original initial-card order
- [x] Implement middle-card access
- [x] Add automated tests for the initial deal
- [x] Add initial architecture and rules documentation

### Current Test Baseline

The current test suite should remain green before moving to the next milestone.

Current baseline:

- 18 tests
- 18 passing
- 0 failing

---

## Phase 2 — Hakem Decision

Implement the state in which the Hakem determines Hokm.

### Tasks

- [ ] Define Hakem decision state
- [ ] Allow Hakem to choose Hokm directly
- [ ] Allow Hakem to request teammate's middle card
- [ ] Reveal the requested middle card publicly
- [ ] Set Hokm from the revealed card's suit
- [ ] Implement Naras
- [ ] Implement Saras
- [ ] Respect `AllowNaras`
- [ ] Respect `AllowSaras`
- [ ] Prevent invalid or duplicate Hakem decisions
- [ ] Add automated tests for every decision path

---

## Phase 3 — Complete Deal

After Hokm has been established:

- [ ] Deal remaining cards in batches of four
- [ ] Continue until every player has 13 cards
- [ ] Preserve required game-state information
- [ ] Define when hand sorting becomes legal
- [ ] Add automated tests for the complete 13-card deal

---

## Phase 4 — Trick Engine

Implement the rules for playing Tricks.

### Tasks

- [ ] Define Trick state
- [ ] Hakem starts the first Trick
- [ ] Determine the lead suit
- [ ] Enforce follow-suit rules
- [ ] Determine legal cards
- [ ] Implement trump behavior
- [ ] Determine Trick winner
- [ ] Transfer Trick result to the winning team
- [ ] Determine when the Hand ends
- [ ] Add comprehensive Trick tests

---

## Phase 5 — Hand Scoring

Implement scoring for an individual Hand.

### Tasks

- [ ] Count Tricks won by each team
- [ ] Determine Hand winner
- [ ] Implement any required Hokm-specific scoring rules
- [ ] Add scoring configuration where appropriate
- [ ] Add automated scoring tests

---

## Phase 6 — Match Engine

Implement the complete Match.

### Tasks

- [ ] Track Hands won by each team
- [ ] Support Match lengths of 1, 3, 5, and 7 Hands
- [ ] Determine Match winner
- [ ] Implement Hakem rotation
- [ ] Respect clockwise/counter-clockwise Hakem rotation
- [ ] Add Match-level tests

---

## Phase 7 — Bot Players

Implement bots so a complete Match can run without human players.

### Tasks

- [ ] Define bot decision interface
- [ ] Implement legal-card selection
- [ ] Implement Hakem decision behavior
- [ ] Implement Trick-play behavior
- [ ] Implement configurable bot difficulty/strategy
- [ ] Simulate complete Matches
- [ ] Add deterministic simulation support for testing

The bot system should use the same game-engine interfaces as human players.

---

## Phase 8 — Console Simulation

Build the console application into a useful development simulator.

### Tasks

- [ ] Start a complete Match
- [ ] Display players and teams
- [ ] Display Hakem decisions
- [ ] Display Tricks
- [ ] Display scores
- [ ] Run bot-vs-bot Matches
- [ ] Add deterministic seeds for reproducible simulations
- [ ] Use the console to debug rule behavior

---

## Phase 9 — Multiplayer Backend

Only after the game engine is reliable.

### Planned Technology

- ASP.NET Core
- SignalR
- PostgreSQL

### Tasks

- [ ] Define server-side game sessions
- [ ] Define room/lobby model
- [ ] Connect players to game sessions
- [ ] Synchronize authoritative game state
- [ ] Implement commands and events
- [ ] Handle reconnects
- [ ] Introduce bot takeover after disconnect timeout
- [ ] Allow returning players to regain control
- [ ] Add authentication/account infrastructure

The backend must not duplicate Hokm rules. The existing game engine remains the authoritative rules layer.

---

## Phase 10 — Flutter Client

Build the mobile client after the multiplayer protocol is stable.

### Targets

- [ ] iOS
- [ ] Android

### Tasks

- [ ] Lobby UI
- [ ] Private room creation
- [ ] Player seating
- [ ] Card table UI
- [ ] Hand display
- [ ] Hakem decision UI
- [ ] Trick animation
- [ ] Score display
- [ ] Reconnection UI
- [ ] Bot takeover indication
- [ ] Spectator support later

---

## Phase 11 — Future Games

Once the platform architecture is stable, additional Persian card games can be added.

Potential games include:

- Pasur
- Shelem
- Other Persian card games

Each game should implement its own rules while reusing appropriate generic infrastructure from `Atlas.Core`.

The platform should avoid putting game-specific rules into shared infrastructure unless those rules are genuinely common across games.

---

## Phase 12 — Future Features

Potential future features include:

- [ ] Spectators
- [ ] Match history
- [ ] Player statistics
- [ ] Friends
- [ ] Private invitations
- [ ] Public matchmaking
- [ ] Tournament support
- [ ] Additional game variants
- [ ] Localization
- [ ] Accessibility improvements

Real-money functionality is intentionally outside the current scope and should not influence the architecture of the current game-engine milestones.